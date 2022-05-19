const cells = document.querySelectorAll('.cell')
const timeBlock = document.querySelector('#finalSetTime')
const badges = document.querySelectorAll('.badge')
var timeId = 1

for (let badge of badges) {
    badge.innerText = 5
    var bdg = parseInt(badge.innerText)
}

for (let cell of cells) {
    cell.addEventListener('click', event => {
        if (cell.classList.contains('selected')) {
            removeElement(cell.id.split("-")[1])

        } else if (cell.className == 'cell' && timeId <= bdg) {
            cell.classList.add('selected');
            cell.id = `cell-${timeId}`
            createElement(timeId);
            timeId += 1

            for (let badge of badges) {
                badge.innerText -= 1
            }

        }
    })
}

function createElement(id) {
    timeBlock.insertAdjacentHTML('beforeend', `<div class="time" id="setTime${id}"><span>${id} • Tue, 5 April • 07:30 - 08:30<span aria-hidden="true"class="fs-20 ms-1" onclick="removeElement(${id})" id="removeSetTime${id}">&times;</span></span></div>`)
}

function removeElement(id) {
    for (let badge of badges) {
        badge.innerText = parseInt(badge.innerText) + 1
    }
    document.getElementById(`setTime${id}`).remove()
    document.getElementById(`cell-${id}`).classList.remove('selected')
    document.getElementById(`cell-${id}`).id = ''
    timeId -= 1
}