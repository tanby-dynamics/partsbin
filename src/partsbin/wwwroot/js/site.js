
if (window.Blazor === undefined) window.Blazor = {};

window.Blazor.resetInputFile = function(id) {
    const inputFile = document.getElementById(id);
    inputFile.value = '';
};
window.Blazor.hideNavbar = function() {
    document
        .querySelector('#offcanvasNavbar > div.offcanvas-header > button')
        .click();
};
window.Blazor.downloadFile = function(filename, dataUrl) {
    const link = document.createElement('a');

    link.href = dataUrl;
    link.download = filename;

    document.body.appendChild(link);

    link.click();

    document.body.removeChild(link);
};

