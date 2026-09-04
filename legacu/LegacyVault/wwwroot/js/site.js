// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const sidebar = document.querySelector("#primary-sidebar");
const menuButtons = document.querySelectorAll(".menu-toggle, .mobile-menu-toggle");

menuButtons.forEach((button) => {
	button.addEventListener("click", () => {
		const isOpen = sidebar.classList.toggle("is-open");
		menuButtons.forEach((menuButton) => menuButton.setAttribute("aria-expanded", isOpen));
	});
});
