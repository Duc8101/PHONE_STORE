$(document).ready(function () {
    function ListAllProduct(name, page) {
        var url = 'https://localhost:7225/Product/Home/List'
        if (name === undefined || name.toString().trim().length === 0) {
            url = url + `?page=${page}`;
        } else {
            url = url + `?name=${name.toString().trim()}&page=${page}`;
        }
        $.ajax({
            url: url,
            method: 'GET',
            success: function (response) {
                // get list product
                var products = response.data.list;
                // traverse each product
                products.forEach(function (product) {
                    var row =
                        `<tr>
                        <td>${product.productName}</td>
                        <td><img src="${product.image}" alt="" width="50" height="50" ></td>
                        <td>${product.price}</td>
                        <td>${product.quantity}</td>
                        <td>${product.categoryName}</td>
                    </tr>`;
                    $('#tableBody').append(row);
                })
            },

            error: function (error) {
                console.error('Error: ', error);
            },

        })
    }

    ListAllProduct(undefined, 1);

    $('#CategoryID').append('<option value="">ALL</option>');

    function ListAllCategory() {
        $.ajax({
            url: 'https://localhost:7225/Category/List/All',
            method: 'GET',
            success: function (response) {
                var categories = response.data;
                categories.forEach(function (category) {
                    var option = `<option value="${category.categoryId}">${category.categoryName}</option>`;
                    $('#CategoryID').append(option);
                })
            },
            error: function (error) {
                console.log(error);
            }
        })
    }

    ListAllCategory();

    function AddPagination(name, page) {
        var url = 'https://localhost:7225/Product/Home/List'
        if (name === undefined || name.toString().trim().length === 0) {
            url = url + `?page=${page}`;
        } else {
            url = url + `?name=${name.toString().trim()}&page=${page}`;
        }
        $.ajax({
            url: url,
            method: 'GET',
            success: function (response) {
                var data = response.data;
                if (data.numberPage > 1) {
                    var liFirst = `<li class="page-item ${data.pageSelected == 1 ? 'disabled' : ''}"><a class="page-link btn btn-primary btn-lg ${data.pageSelected == 1 ? 'disabled' : ''}" href="#" aria-disabled="true" role="button">First</a></li>`;
                    $('#pagination').append(liFirst);
                    var liPre = `<li class="page-item ${data.pageSelected == 1 ? 'disabled' : ''}"><a class="page-link btn btn-primary btn-lg ${data.pageSelected == 1 ? 'disabled' : ''}" href="#" aria-disabled="true" role="button">Previous</a></li>`;
                    $('#pagination').append(liPre);
                    var liNumPage = `<li class="page-item"><a href="" class="page-link btn btn-primary btn-lg">${data.pageSelected}/${data.numberPage}</a></li>`;
                    $('#pagination').append(liNumPage);
                    var liNext = `<li class="page-item ${data.pageSelected == data.numberPage ? 'disabled' : ''}"><a class="page-link btn btn-primary btn-lg ${data.pageSelected == data.numberPage ? 'disabled' : ''}" href="#" aria-disabled="true" role="button">Next</a></li>`;
                    $('#pagination').append(liNext);
                    var liLast = `<li class="page-item ${data.pageSelected == data.numberPage ? 'disabled' : ''}"><a class="page-link btn btn-primary btn-lg ${data.pageSelected == data.numberPage ? 'disabled' : ''}" href="#" aria-disabled="true" role="button">Last</a></li>`;
                    $('#pagination').append(liLast);
                }
            },

            error : function(error){
                console.log(error);
            }
        })
    }

    AddPagination(undefined, 1);
})