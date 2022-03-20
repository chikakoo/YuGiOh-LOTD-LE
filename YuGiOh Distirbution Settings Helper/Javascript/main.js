let Main = {
    deck1: null,
    deck2: null,
    selectedComponent: null,

    initialize: function() {
        let decksContainer = document.getElementById("decksContainer");

        this.deck1 = new BaseDeck();
        this.deck2 = new BaseDeck();

        this.deck2.mainDeckSize.max = 60;
        decksContainer.appendChild(this.deck1.getDisplayDiv());
        decksContainer.appendChild(this.deck2.getDisplayDiv());

        ContextMenu.initialize();
    },

    /**
     * Adds the component to the edit div
     * @param {Any} component - the component to edit
     */
    editComponent: function(component) {
        this.clearEditComponent();

        this.selectedComponent = component;
        addCssClass(this.selectedComponent.displayDiv, "display-selected");

        let editContainer = document.getElementById("editContainer");
        editContainer.appendChild(component.getEditDiv());
    },

    /**
     * Clears out the edit component window
     */
    clearEditComponent: function() {
        if (this.selectedComponent) {
            this.refreshDisplay(); // Refresh with any changes before we switch out the components
            removeCssClass(this.selectedComponent.displayDiv, "display-selected");
        }

        let editContainer = document.getElementById("editContainer");
        editContainer.innerHTML = "";
    },

    refreshDisplay: function() {
        if (!this.selectedComponent) {
            return;
        }

        this.selectedComponent.refreshMainDisplayDiv();
    }
};