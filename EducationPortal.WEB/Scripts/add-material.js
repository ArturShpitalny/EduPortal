(function (window) {
    let timerId = null;
    let btns = document.getElementsByClassName('add-material-btn');

    for (let i = 0; i < btns.length; i++) {
        btns[i].onclick = function () {
            let postSend = new XMLHttpRequest();
            postSend.onreadystatechange = onReadyStateChange;
            postSend.open('POST', location.protocol + "//" + location.host + '/MaterialCourse/AddMaterialToCourse', true);
            postSend.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            postSend.send('courseid=' + this.getAttribute('courseId') + '&materialid=' + this.getAttribute('materialId'));
        }
    }

    function clearMessage(elem) {
        while (elem.firstChild) {
            elem.removeChild(elem.firstChild);
        }
    };

    function onReadyStateChange() {
        if (this.readyState === 4 && this.status === 200) {
            let messageBlock = document.getElementById('message');

            clearMessage(messageBlock);
            if (timerId != null) {
                clearTimeout(timerId);
            }
            timerId = setTimeout(function () {
                clearMessage(messageBlock);
            }, 3000);

            let fixedTop = document.createElement('div');
            fixedTop.className = "fixed-top";

            let row = document.createElement('div');
            row.className = "row justify-content-center my-3";

            let col = document.createElement('div');
            col.className = "col-2 message px-3";

            let message = document.createElement('div');
            message.className = "text-center";

            try {
                let obj = JSON.parse(this.responseText);

                if (obj.Message == 'OK') {
                    message.textContent = 'Материал добавлен в курс';
                }
                else if (obj.Message == 'MaterialCourseAlreadyAdded') {
                    message.textContent = 'Материал уже был добавлен в курс';
                }
                else if (obj.Message == 'UserNotAuthorize') {
                    message.textContent = 'Войдите или зарегистрируйтесь';
                }
            } catch{
                message.textContent = 'Ошибка';
            }

            col.append(message);
            row.append(col);
            fixedTop.append(row);
            messageBlock.append(fixedTop);
        }
    };
})(window)