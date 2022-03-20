let ContextMenu = {
    _element: null,

    /**
     * Initializes the context menu by assigning the element reference
     */
    initialize: function() {
        this._element = document.getElementById("contextMenu");
    },

    /**
     * Shows the context menu
     * @param {Any} options - the options, which is an array of objects with the following:
     * - text: the text to display
     * - callback: the function to call if clicked
     * - enabled: whether the option is enabled
     * @param {Any} contextMenuEvet - the event from the context menu (for mouse position)
     */
    show: function(options, contextMenuEvent) {
        this._element.innerHTML = "";

        let _this = this;
        options.forEach(function(option) {
            _this._addOption(option);
        });

        this._element.style.left = `${contextMenuEvent.pageX}px`;
        this._element.style.top = `${contextMenuEvent.pageY}px`;

        showElement(this._element);
    },

    /**
     * Add an option to the menu
     * @param {Any} option - the option to add (see above for documentation)
     */
    _addOption: function(option) {
        let optionElement = dce("div", "context-menu-option");
        optionElement.innerText = option.text;
        
        if (option.disabled) {
            addCssClass(optionElement, "context-menu-disabled");
        }

        let _this = this;
        optionElement.onclick = function() {
            if (option.callback && !option.disabled) {
                option.callback();
            }

            _this.hide();
        }

        this._element.appendChild(optionElement);
    },

    /**
     * Hides the context menu
     */
    hide: function() {
        hideElement(this._element);
    }
};