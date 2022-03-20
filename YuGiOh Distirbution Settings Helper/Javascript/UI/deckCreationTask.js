class DeckCreationTask {
    displayDiv = null;
    baseDeckRef = null;
    id = 0; // A reference from the baseDeck - just a running ID
    _nextFilterId = 1;

    generalCardType = Enums.GeneralCardTypes.MONSTER;
    cardRange = { min: 1, max: 1 };
    filterSetType = Enums.FilterSetTypes.ROUND_ROBIN;
    mainFilter = null;
    filterSet = [];

    _summaryDiv = null;
    _filterSetTypeDiv = null;

    constructor(baseDeckRef, id) { 
        this.baseDeckRef = baseDeckRef;
        this.id = id;
        this.mainFilter = new Filter(true, true, this);
    };

    getDisplayDiv = function() {
        this.displayDiv = dce("dce", "deck-display deck-creation-task");
        this._summaryDiv = dce("dce");
        this._filterSetTypeDiv = dce("dce");
        
        let _this = this;
        this.displayDiv.onclick = function(event) {
            event.stopPropagation();
            Main.editComponent(_this);
        };

        this.displayDiv.oncontextmenu = function(event) {
            event.preventDefault();
            event.stopPropagation();
            ContextMenu.show([
                {
                    text: "Add new filter", 
                    callback: function() { 
                        _this.addNewFilter();
                    }
                },
                {
                    text: "Move up",
                    callback: function() {
                        _this.baseDeckRef.moveDeckCreationTaskUp(_this);
                    }
                },
                {
                    text: "Move down",
                    callback: function() {
                        _this.baseDeckRef.moveDeckCreationTaskDown(_this);
                    }
                },
                { 
                    text: "Delete this task", 
                    callback: function() { 
                        _this.baseDeckRef.deleteDeckCreationTask(_this);
                    }
                }
            ], event);
        };

        this.refreshMainDisplayDiv();

        this.displayDiv.appendChild(this._summaryDiv);
        this.displayDiv.appendChild(this._filterSetTypeDiv);
        this.displayDiv.appendChild(this.mainFilter.getDisplayDiv());
        return this.displayDiv;
    };

    refreshMainDisplayDiv = function() {
        let rangeValue = `${this.cardRange.min} - ${this.cardRange.max}`;
        this._summaryDiv.innerText = `${rangeValue}  ${this.generalCardType} cards`;
        this._filterSetTypeDiv.innerText = `${this.filterSetType} filter`;
    };

    addNewFilter = function() {
        let newFilter = new Filter(false, true, this, this._nextFilterId++);
        this.filterSet.push(newFilter);
        this.displayDiv.appendChild(newFilter.getDisplayDiv());
    };

    deleteFilter = function(filter) {
        let index = this.filterSet.indexOf(filter);
        if (index < 0) {
            throw "ERROR: tried to delete non-existant filter!";
        }

        this.filterSet.splice(index, 1);
        filter.displayDiv.remove();

        Main.clearEditComponent(); // Just in case the component that was deleted is active
    };

    moveFilterUp = function(filter) {
        let index = this.filterSet.indexOf(filter);
        if (index <= 0) {
            return;
        }

        let prevIndex = index - 1;
        let prevFilter = this.filterSet[prevIndex];

        this.filterSet[prevIndex] = filter;
        this.filterSet[index] = prevFilter;

        prevFilter.displayDiv.before(filter.displayDiv);
    };

    moveFilterDown = function(filter) {
        let index = this.filterSet.indexOf(filter);
        if (index >= this.filterSet.length - 1) {
            return;
        }

        let nextIndex = index + 1;
        let nextFilter = this.filterSet[nextIndex];

        this.filterSet[nextIndex] = filter;
        this.filterSet[index] = nextFilter;

        filter.displayDiv.before(nextFilter.displayDiv);
    };

    getEditDiv = function() {
        let editDiv = dce("dce");
        let generalCardTypeDiv = UIHelpers.createRadioButtonDiv(this, "General Card Type", "generalCardType", Enums.getAllGeneralCardTypeValues());
        let cardRangeDiv = UIHelpers.createRangeDiv(this, "Card Range", "cardRange", 1, 60);
        let filterSetTypeDiv = UIHelpers.createRadioButtonDiv(this, "Filter Set Type", "filterSetType", Object.values(Enums.FilterSetTypes));

        editDiv.appendChild(generalCardTypeDiv);
        editDiv.appendChild(cardRangeDiv);
        editDiv.appendChild(filterSetTypeDiv);
        return editDiv;
    };
}