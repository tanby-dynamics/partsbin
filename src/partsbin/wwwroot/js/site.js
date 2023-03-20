// See NavService
function hideNavbar() {
    document
        .querySelector('#offcanvasNavbar > div.offcanvas-header > button')
        .click();
}

// See FileService
function downloadFile(filename, dataUrl) {
    const link = document.createElement('a');
    
    link.href = dataUrl;
    link.download = filename;
    
    document.body.appendChild(link);
    
    link.click();
    
    document.body.removeChild(link);
}
