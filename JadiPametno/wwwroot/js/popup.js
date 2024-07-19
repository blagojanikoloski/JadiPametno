document.addEventListener("DOMContentLoaded", function () {
    // JavaScript to toggle the visibility of the pop-up container
    var openPopupButton = document.getElementById("calculate-button");
    var closePopupButton = document.getElementById("close-button");
    var popupContainer = document.getElementById("popupContainer");
    var overlay = document.getElementById("popup-overlay");

    openPopupButton.addEventListener("click", function () {
        if (popupContainer.style.display === "none" || !popupContainer.style.display) {
            popupContainer.style.display = "flex";
            overlay.style.display = "block";
        } 
    });

    closePopupButton.addEventListener("click", function () {
        popupContainer.style.display = "none";
        overlay.style.display = "none";
    });
});
