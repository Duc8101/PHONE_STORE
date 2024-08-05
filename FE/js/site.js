const role = sessionStorage.getItem('role');
const username = sessionStorage.getItem('username');
// if admin
if (role === "1") {
    document.getElementById('manager').style.display = 'flex';
    document.getElementById('home').style.display = 'none';
} else if (role === "2") {
    document.getElementById('manager').style.display = 'none';
    document.getElementById('home').style.display = 'flex';
} else {
    document.getElementById('manager').style.display = 'none';
    document.getElementById('home').style.display = 'none';
}

// if customer
if (role === "2") {
    document.getElementById('ulCart').style.display = 'flex';
    document.getElementById('liProfile').style.display = 'flex';
    document.getElementById('liMyOrder').style.display = 'flex';
    document.getElementById('liChangePassword').style.display = 'none';
} else if (role === 1) {
    document.getElementById('ulCart').style.display = 'none';
    document.getElementById('liProfile').style.display = 'none';
    document.getElementById('liMyOrder').style.display = 'none';
    document.getElementById('liChangePassword').style.display = 'flex';
} else {
    document.getElementById('ulCart').style.display = 'none';
    document.getElementById('liProfile').style.display = 'none';
    document.getElementById('liMyOrder').style.display = 'none';
    document.getElementById('liChangePassword').style.display = 'none';
}

document.getElementById('navUser').style.display = username === null ? 'none' : 'flex';
document.getElementById('welcome').innerHTML = `Welcome ${username}`;