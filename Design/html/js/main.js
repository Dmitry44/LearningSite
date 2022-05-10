var fn_time = document.querySelector('.final-set-time');
var badge = document.getElementById("badge_qnt");


document.querySelectorAll('.cell').forEach(item => {
    item.addEventListener('click', event => {
        if (item.className == "cell" && parseInt(badge.innerText) > 0) {
            item.className += " selected";
            badge.innerText = parseInt(badge.innerText) - 1;
            fn_time.innerHTML += '<div class="time"><span>Tue, 5 April • 07:30 - 08:30<span aria-hidden="true" class="fs-20 ms-1">&times;</span></span></div>';
        }
    })
})