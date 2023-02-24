// https://www.puresourcecode.com/dotnet/blazor/create-a-blazor-component-for-quill/
(function () {
    window.QuillFunctions = {
        createQuill: function (quillElement, toolBar, readOnly, placeholder, theme, debugLevel) {
            var options = {
                debug: debugLevel,
                modules: {
                    toolbar: toolBar
                },
                placeholder: placeholder,
                readOnly: readOnly,
                theme: theme
            };

            new Quill(quillElement, options);
        },
        getQuillContent: function (quillElement) {
            return JSON.stringify(quillElement.__quill.getContents());
        },
        getQuillText: function (quillElement) {
            return quillElement.__quill.getText();
        },
        getQuillHTML: function (quillElement) {
            return quillElement.__quill.root.innerHTML;
        },
        loadQuillContent: function (quillElement, quillContent) {
            var content = JSON.parse(quillContent);
            
            // WARNING: Hacky hacky hack hack
            function setContents() {
                if (quillElement.__quill === undefined) return setTimeout(setContents, 10);
                
                return quillElement.__quill.setContents(content, 'api'); 
            }
            setContents();
        },
        enableQuillEditor: function (quillElement, mode) {
            quillElement.__quill.enable(mode);
        }
    };
})();