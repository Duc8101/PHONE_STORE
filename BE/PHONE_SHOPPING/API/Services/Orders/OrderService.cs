using API.Services.Base;
using AutoMapper;
using Common.Base;
using Common.DTO.CartDTO;
using Common.DTO.OrderDetailDTO;
using Common.DTO.OrderDTO;
using Common.Entity;
using Common.Enums;
using Common.Paginations;
using DataAccess.DBContext;
using DataAccess.Helper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Services.Orders
{
    public class OrderService : BaseService, IOrderService
    {
        public OrderService(IMapper mapper, PHONE_STOREContext context) : base(mapper, context)
        {

        }

        public async Task<ResponseBase> Create(OrderCreateDTO DTO, Guid userId)
        {
            try
            {
                List<Cart> list = _context.Carts.Include(c => c.Product).Where(c => c.UserId == userId && c.IsCheckOut == false && c.IsDeleted == false).ToList();
                List<CartListDTO> data = _mapper.Map<List<CartListDTO>>(list);
                if (DTO.Address == null || DTO.Address.Trim().Length == 0)
                {
                    return new ResponseBase(data, "You have to input address", (int)HttpStatusCode.Conflict);
                }
                foreach (CartListDTO item in data)
                {
                    Product? product = _context.Products.Include(p => p.Category).SingleOrDefault(p => p.ProductId == item.ProductId && p.IsDeleted == false);
                    if (product == null)
                    {
                        return new ResponseBase(data, "Product " + item.ProductName + " not exist!!!", (int)HttpStatusCode.NotFound);
                    }
                    if (product.Quantity < item.Quantity)
                    {
                        return new ResponseBase(data, "Product " + item.ProductName + " not have enough quantity!!!", (int)HttpStatusCode.Conflict);
                    }
                }
                string body = UserHelper.BodyEmailForAdminReceiveOrder();
                List<string> emails = _context.Users.Where(u => u.RoleId == (int)Roles.Admin).Select(u => u.Email).ToList();
                if (emails.Count > 0)
                {
                    foreach (string email in emails)
                    {
                        await UserHelper.sendEmail("[PHONE SHOPPING] Notification for new order", body, email);
                    }
                }
                Order order = _mapper.Map<Order>(DTO);
                order.OrderId = Guid.NewGuid();
                order.Address = DTO.Address.Trim();
                order.Status = OrderStatus.Pending.ToString();
                order.CreatedAt = DateTime.Now;
                order.UpdateAt = DateTime.Now;
                order.IsDeleted = false;
                order.Note = null;
                order.UserId = userId;
                _context.Orders.Add(order);
                _context.SaveChanges();
                foreach (CartListDTO item in data)
                {
                    OrderDetail detail = _mapper.Map<OrderDetail>(item);
                    detail.DetailId = Guid.NewGuid();
                    detail.OrderId = order.OrderId;
                    detail.CreatedAt = DateTime.Now;
                    detail.UpdateAt = DateTime.Now;
                    detail.IsDeleted = false;
                    _context.OrderDetails.Add(detail);
                    _context.SaveChanges();
                }
                foreach (Cart cart in list)
                {
                    cart.IsCheckOut = true;
                    _context.Carts.Update(cart);
                    _context.SaveChanges();
                }
                return new ResponseBase(data, "Check out successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        private IQueryable<Order> getQuery(Guid? userId, string? status)
        {
            IQueryable<Order> query = _context.Orders.Include(u => u.User);
            if (userId.HasValue)
            {
                query = query.Where(o => o.UserId == userId);
            }
            if (status != null && status.Trim().Length > 0)
            {
                query = query.Where(o => o.Status == status.Trim());
            }
            query = query.OrderBy(o => o.Status == OrderStatus.Pending.ToString() ? 0 : 1).ThenByDescending(o => o.UpdateAt);
            return query;
        }

        public ResponseBase List(Guid? userId, string? status, bool isAdmin, int page)
        {
            try
            {
                IQueryable<Order> query = getQuery(userId, status);
                List<Order> list = query.Skip((int)PageSize.Order_List * (page - 1)).Take((int)PageSize.Order_List)
                    .ToList();
                List<OrderListDTO> DTO = _mapper.Map<List<OrderListDTO>>(list);
                int count = query.Count();
                int number = (int)Math.Ceiling((double)count / (int)PageSize.Order_List);
                int prePage = page - 1;
                int nextPage = page + 1;
                string preURL;
                string nextURL;
                string firstURL;
                string lastURL;
                if (isAdmin)
                {
                    if (status == null || status.Trim().Length == 0)
                    {
                        preURL = "/ManagerOrder?page=" + prePage;
                        nextURL = "/ManagerOrder?page=" + nextPage;
                        firstURL = "/ManagerOrder";
                        lastURL = "/ManagerOrder?page=" + number;
                    }
                    else
                    {
                        preURL = "/ManagerOrder?status=" + status.Trim() + "&page=" + prePage;
                        nextURL = "/ManagerOrder?status=" + status.Trim() + "&page=" + nextPage;
                        firstURL = "/ManagerOrder?status=" + status.Trim();
                        lastURL = "/ManagerOrder?status=" + status.Trim() + "&page=" + number;
                    }
                }
                else
                {
                    preURL = "/MyOrder?page=" + prePage;
                    nextURL = "/MyOrder?page=" + nextPage;
                    firstURL = "/MyOrder";
                    lastURL = "/MyOrder?page=" + number;
                }
                Pagination<OrderListDTO> data = new Pagination<OrderListDTO>()
                {
                    PageSelected = page,
                    NumberPage = number,
                    List = DTO,
                    FIRST_URL = firstURL,
                    LAST_URL = lastURL,
                    NEXT_URL = nextURL,
                    PRE_URL = preURL,
                };
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Detail(Guid orderId, Guid? userId)
        {
            try
            {
                IQueryable<Order> query = _context.Orders.Include(o => o.User)
                    .Include(o => o.OrderDetails).ThenInclude(o => o.Product)
                    .ThenInclude(o => o.Category);
                Order? order;
                if (userId == null)
                {
                    order = query.SingleOrDefault(o => o.OrderId == orderId);
                }
                else
                {
                    order = query.SingleOrDefault(o => o.OrderId == orderId && o.UserId == userId);
                }
                if (order == null)
                {
                    return new ResponseBase("Not found order", (int)HttpStatusCode.NotFound);
                }
                List<OrderDetail> list = order.OrderDetails.ToList();
                List<DetailDTO> DTOs = _mapper.Map<List<DetailDTO>>(list);
                OrderDetailDTO data = _mapper.Map<OrderDetailDTO>(order);
                data.OrderId = orderId;
                data.DetailDTOs = DTOs;
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase> Update(Guid orderId, OrderUpdateDTO DTO)
        {
            try
            {
                Order? order = _context.Orders.Include(o => o.User).Include(o => o.OrderDetails).ThenInclude(o => o.Product)
                    .ThenInclude(o => o.Category).SingleOrDefault(o => o.OrderId == orderId);
                if (order == null)
                {
                    return new ResponseBase("Not found order", (int)HttpStatusCode.NotFound);
                }
                List<OrderDetail> list = order.OrderDetails.ToList();
                List<DetailDTO> DTOs = _mapper.Map<List<DetailDTO>>(list);
                OrderDetailDTO data = _mapper.Map<OrderDetailDTO>(order);
                data.OrderId = orderId;
                data.DetailDTOs = DTOs; 
                if (order.Status == OrderStatus.Rejected.ToString() || order.Status == OrderStatus.Approved.ToString())
                {
                    return new ResponseBase(data, "Order was approved or rejected", (int)HttpStatusCode.Conflict);
                }
                if (DTO.Status.Trim() == OrderStatus.Rejected.ToString() || DTO.Status.Trim() == OrderStatus.Pending.ToString())
                {
                    if (DTO.Status.Trim() == OrderStatus.Rejected.ToString() && (DTO.Note == null || DTO.Note.Trim().Length == 0))
                    {
                        return new ResponseBase(data, "When status is " + OrderStatus.Rejected + ", you have to note the reason why rejected", (int)HttpStatusCode.Conflict);
                    }
                    order.Status = DTO.Status.Trim();
                    order.UpdateAt = DateTime.Now;
                    order.Note = DTO.Note == null || DTO.Note.Trim().Length == 0 ? null : DTO.Note.Trim();
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                    data.Status = order.Status;
                    data.Note = order.Note;
                    return new ResponseBase(data, "Update successful");
                }
                if (DTO.Status.Trim() == OrderStatus.Approved.ToString())
                {
                    order.Status = DTO.Status.Trim();
                    order.Note = DTO.Note == null || DTO.Note.Trim().Length == 0 ? null : DTO.Note.Trim();
                    data.Status = order.Status;
                    data.Note = order.Note;
                    foreach (OrderDetail item in list)
                    {
                        Product? product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == item.ProductId && p.IsDeleted == false);
                        if (product == null)
                        {
                            return new ResponseBase(data, "Product " + item.Product.ProductName + " not exist!!!", (int)HttpStatusCode.Conflict);
                        }
                        if (product.Quantity < item.Quantity)
                        {
                            return new ResponseBase(data, "Product " + item.Product.ProductName + " not have enough quantity!!!", (int)HttpStatusCode.Conflict);
                        }
                    }
                    string body = UserHelper.BodyEmailForApproveOrder(list);
                    await UserHelper.sendEmail("[PHONE SHOPPING] Notification for approve order", body, order.User.Email);
                    foreach (OrderDetail item in list)
                    {
                        item.Product.Quantity = item.Product.Quantity - item.Quantity;
                        item.Product.UpdateAt = DateTime.Now;
                        _context.SaveChanges();
                    }
                    order.UpdateAt = DateTime.Now;
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                    return new ResponseBase(data, "Update successful");
                }
                return new ResponseBase(data, "Status update must be " + OrderStatus.Approved + "," + OrderStatus.Rejected + " or " + OrderStatus.Pending, (int)HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
