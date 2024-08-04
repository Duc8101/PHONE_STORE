$(document).ready(function () {
    // set pagination
    function SetPagination(data) {
        document.getElementById('urlNumber').innerHTML = `${data.pageSelected}/${data.numberPage}`;
        const pagination = document.getElementById('pagination');
        if (data.numberPage > 1) {
            if (pagination.style.display === 'none') {
                pagination.style.display = 'flex';
            }
            if (data.pageSelected === 1) {
                document.getElementById("liFirst").classList.add("disabled");
                document.getElementById("urlFirst").classList.add("disabled");
                document.getElementById("liPrevious").classList.add("disabled");
                document.getElementById("urlPrevious").classList.add("disabled");
                document.getElementById("liNext").classList.remove("disabled");
                document.getElementById("urlNext").classList.remove("disabled");
                document.getElementById("liLast").classList.remove("disabled");
                document.getElementById("urlLast").classList.remove("disabled");
            } else if (data.pageSelected === data.numberPage) {
                document.getElementById("liNext").classList.add("disabled");
                document.getElementById("urlNext").classList.add("disabled");
                document.getElementById("liLast").classList.add("disabled");
                document.getElementById("urlLast").classList.add("disabled");
                document.getElementById("liFirst").classList.remove("disabled");
                document.getElementById("urlFirst").classList.remove("disabled");
                document.getElementById("liPrevious").classList.remove("disabled");
                document.getElementById("urlPrevious").classList.remove("disabled");
            } else {
                document.getElementById("liFirst").classList.remove("disabled");
                document.getElementById("urlFirst").classList.remove("disabled");
                document.getElementById("liPrevious").classList.remove("disabled");
                document.getElementById("urlPrevious").classList.remove("disabled");
                document.getElementById("liNext").classList.remove("disabled");
                document.getElementById("urlNext").classList.remove("disabled");
                document.getElementById("liLast").classList.remove("disabled");
                document.getElementById("urlLast").classList.remove("disabled");
            }
        } else {
            pagination.style.display = 'none';
        }
    }

    // list all product
    function ListAllProduct(name, categoryId, page) {
        var url = 'https://localhost:7225/Product/List'
        if ((name === null || name.toString().trim().length === 0) && categoryId === 0) {
            url = url + `?page=${page}`;
        } else if (name === null || name.toString().trim().length === 0) {
            url = url + `?categoryId=${categoryId}&page=${page}`;
        } else if (categoryId === 0) {
            url = url + `?name=${name.toString().trim()}&page=${page}`;
        } else {
            url = url + `?name=${name.toString().trim()}&categoryId=${categoryId}&page=${page}`;
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
                SetPagination(response.data);
                document.getElementById('numberPage').value = response.data.numberPage;
            },

            error: function (error) {
                console.error('Error: ', error);
            },

        })
    }

    $('#CategoryId').append('<option value="0">ALL</option>');

    // list all category
    function ListAllCategory() {
        $.ajax({
            url: 'https://localhost:7225/Category/List/All',
            method: 'GET',
            success: function (response) {
                var categories = response.data;
                categories.forEach(function (category) {
                    var option = `<option value="${category.categoryId}">${category.categoryName}</option>`;
                    $('#CategoryId').append(option);
                })
            },
            error: function (error) {
                console.log(error);
            }
        })
    }

    ListAllCategory();

    ListAllProduct(null, 0, 1);

    // ---------------------- script when click button search -----------------------
    var search = document.getElementById('search');
    search.addEventListener('click', function () {
        var searchName = document.getElementById('Name');
        var name = searchName.value;
        var searchCategory = document.getElementById('CategoryId');
        var categoryId = parseInt(searchCategory.value, 10);
        document.getElementById('CategoryId').value = categoryId;
        $('#tableBody').empty();
        ListAllProduct(name, categoryId, 1);
    });

    // ---------------------- script when click url Next-----------------------
    var urlNext = document.getElementById('urlNext');
    urlNext.addEventListener('click', function () {
        var searchName = document.getElementById('Name');
        var name = searchName.value;
        var searchCategory = document.getElementById('CategoryId');
        var categoryId = parseInt(searchCategory.value, 10);
        document.getElementById('CategoryId').value = categoryId;
        var currentPage = document.getElementById('currentPage');
        var pageValue = currentPage.value;
        var pageSelected = parseInt(pageValue, 10) + 1;
        document.getElementById('currentPage').value = pageSelected;
        $('#tableBody').empty();
        ListAllProduct(name, categoryId, pageSelected);
    });

    // ---------------------- script when click url Previous-----------------------
    var urlPrevious = document.getElementById('urlPrevious');
    urlPrevious.addEventListener('click', function () {
        var searchName = document.getElementById('Name');
        var name = searchName.value;
        var currentPage = document.getElementById('currentPage');
        var searchCategory = document.getElementById('CategoryId');
        var categoryId = parseInt(searchCategory.value, 10);
        document.getElementById('CategoryId').value = categoryId;
        var pageValue = currentPage.value;
        var pageSelected = parseInt(pageValue, 10) - 1;
        document.getElementById('currentPage').value = pageSelected;
        $('#tableBody').empty();
        ListAllProduct(name, categoryId, pageSelected);
    });

    // ---------------------- script when click url First-----------------------
    var urlFirst = document.getElementById('urlFirst');
    urlFirst.addEventListener('click', function () {
        var searchName = document.getElementById('Name');
        var name = searchName.value;
        var searchCategory = document.getElementById('CategoryId');
        var categoryId = parseInt(searchCategory.value, 10);
        document.getElementById('CategoryId').value = categoryId;
        document.getElementById('currentPage').value = 1;
        $('#tableBody').empty();
        ListAllProduct(name, categoryId, 1);
    });

    // ---------------------- script when click url Last-----------------------
    var urlLast = document.getElementById('urlLast');
    urlLast.addEventListener('click', function () {
        var searchName = document.getElementById('Name');
        var name = searchName.value;
        var searchCategory = document.getElementById('CategoryId');
        var categoryId = parseInt(searchCategory.value, 10);
        document.getElementById('CategoryId').value = categoryId;
        var numberPage = document.getElementById("numberPage");
        document.getElementById('currentPage').value = numberPage.value;
        $('#tableBody').empty();
        ListAllProduct(name, categoryId, numberPage.value);
    });

})
