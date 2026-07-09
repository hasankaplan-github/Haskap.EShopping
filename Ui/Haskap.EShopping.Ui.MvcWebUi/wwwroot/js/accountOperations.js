let searchResultTable = null;

function showUpdatePermissionsModal() {
    $.ajax({
        type: "GET",
        url: '/Admin/Account/LoadUpdatePermissionsViewComponent', //'@Url.Action("LoadCreateReservationViewComponent", "Schedule")',
        //data: {
        //    userId: userId
        //}
    }).done(function (result, status, xhr) {
        
        $("#userUpdatePermissionsModalContent").html(result);
        userUpdatePermissionsModal = new bootstrap.Modal(document.getElementById(modal.userUpdatePermissionsModalId), wizardModalOptions);
        userUpdatePermissionsModal.show();
    });
}

function showUpdatePermissionsModalWithSameTenant(userId) {
    $.ajax({
        type: "GET",
        url: '/Admin/Account/LoadUpdatePermissionsViewComponentWithSameTenant', //'@Url.Action("LoadCreateReservationViewComponent", "Schedule")',
        data: {
            userId: userId
        }
    }).done(function (result, status, xhr) {
        $("#userUpdatePermissionsModalContent").html(result);
        userUpdatePermissionsModal = new bootstrap.Modal(document.getElementById(modal.userUpdatePermissionsModalId), wizardModalOptions);
        userUpdatePermissionsModal.show();
    });
}

function hideUserUpdatePermissionsModal() {
    userUpdatePermissionsModal.hide();
}

function showUpdateRolesModal() {
    $.ajax({
        type: "GET",
        url: '/Admin/Account/LoadUpdateRolesViewComponent', //'@Url.Action("LoadCreateReservationViewComponent", "Schedule")',
        //data: {
        //    userId: userId
        //}
    }).done(function (result, status, xhr) {
        $("#userUpdateRolesModalContent").html(result);
        userUpdateRolesModal = new bootstrap.Modal(document.getElementById(modal.userUpdateRolesModalId), wizardModalOptions);
        userUpdateRolesModal.show();
    });
}

function showUpdateRolesModalWithSameTenant(userId) {
    $.ajax({
        type: "GET",
        url: '/Admin/Account/LoadUpdateRolesViewComponentWithSameTenant', //'@Url.Action("LoadCreateReservationViewComponent", "Schedule")',
        data: {
            userId: userId
        }
    }).done(function (result, status, xhr) {
        $("#userUpdateRolesModalContent").html(result);
        userUpdateRolesModal = new bootstrap.Modal(document.getElementById(modal.userUpdateRolesModalId), wizardModalOptions);
        userUpdateRolesModal.show();
    });
}

function hideUserUpdateRolesModal() {
    userUpdateRolesModal.hide();
}
