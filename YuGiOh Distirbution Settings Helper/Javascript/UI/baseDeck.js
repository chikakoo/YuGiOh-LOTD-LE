class BaseDeck {
    mainDeckSize = { min: 40, max: 40 };
    extraDeckSize = { min: 15, max : 15 };
    mainDeckAddRandomCardsIfNeeded = true;
    extraDeckAddRandomCardsIfNeeded = false;
    ignoreBanList = true;

    mainFilter = new Filter(true);
    deckCreationTasks = [];

    displayDiv = null;
    _mainDeckSizeDiv = null;
    _extraDeckSizeDiv = null;
    _randomCardsDiv = null;
    _banListDiv = null;
    _deckCreationTasksDiv = null;
    _currentTaskId = 1;

    constructor() { }

    getDisplayDiv = function() {
        this.displayDiv = dce("div", "deck-display");
        let deckSizeContainer = this._createDeckSizeDiv();
        this._createAddRandomCardsDiv();
        this._createBanListDiv();

        let _this = this;
        this.displayDiv.onclick = function() {
            Main.editComponent(_this);
        };

        this.refreshMainDisplayDiv();

        this.displayDiv.appendChild(deckSizeContainer);
        this.displayDiv.appendChild(this._randomCardsDiv);
        this.displayDiv.appendChild(this._banListDiv);
        this.displayDiv.appendChild(this.mainFilter.getDisplayDiv());
        this._getAndAppendDeckCreationTaskDiv();

        return this.displayDiv;
    };

    _getAndAppendDeckCreationTaskDiv = function() {
        this._deckCreationTasksDiv = dce("div", "deck-display deck-creation-task-container");
        this._deckCreationTasksDiv.innerText = "Deck Creation Tasks";

        this._deckCreationTasksDiv.onclick = function(event) {
            event.stopPropagation();
        }

        let _this = this;
        this._deckCreationTasksDiv.oncontextmenu = function(event) {
            ContextMenu.show([
                { 
                    text: "Create new task", 
                    callback: function() { 
                        _this._addNewDeckCreationTask();
                    }
                }
            ], event);
        };

        this.displayDiv.appendChild(this._deckCreationTasksDiv);
    };

    _addNewDeckCreationTask() {
        let newTask = new DeckCreationTask(this, this._currentTaskId++);
        this.deckCreationTasks.push(newTask);
        this._deckCreationTasksDiv.appendChild(newTask.getDisplayDiv());
    };

    moveDeckCreationTaskUp = function(task) {
        let index = this.deckCreationTasks.indexOf(task);
        if (index <= 0) {
            return;
        }

        let prevIndex = index - 1;
        let prevTask = this.deckCreationTasks[prevIndex];

        this.deckCreationTasks[prevIndex] = task;
        this.deckCreationTasks[index] = prevTask;

        prevTask.displayDiv.before(task.displayDiv);
    };

    moveDeckCreationTaskDown = function(task) {
        let index = this.deckCreationTasks.indexOf(task);
        if (index >= this.deckCreationTasks.length - 1) {
            return;
        }

        let nextIndex = index + 1;
        let nextTask = this.deckCreationTasks[nextIndex];

        this.deckCreationTasks[nextIndex] = task;
        this.deckCreationTasks[index] = nextTask;

        task.displayDiv.before(nextTask.displayDiv);
    };

    deleteDeckCreationTask = function(task) {
        let index = this.deckCreationTasks.indexOf(task);

        if (index < 0) {
            throw "ERROR: tried to delete non-existant task!";
        }

        this.deckCreationTasks.splice(index, 1);
        task.displayDiv.remove();

        Main.clearEditComponent(); // Just in case the component that was deleted is active
    };

    _createDeckSizeDiv = function() {
        this._mainDeckSizeDiv = dce("div");
        this._extraDeckSizeDiv = dce("div");

        let deckSizeContainer = dce("div", "deck-size-container");
        deckSizeContainer.appendChild(this._mainDeckSizeDiv);
        deckSizeContainer.appendChild(this._extraDeckSizeDiv);

        return deckSizeContainer;
    };

    _createAddRandomCardsDiv = function() {
        this._randomCardsDiv = dce("div");
    };

    _createBanListDiv = function() {
        this._banListDiv = dce("div");
    };

    /**
     * Refreshes the main display div - doesn't include the filter
     */
    refreshMainDisplayDiv = function() {
        this._refreshDeckSizeDivs();
        this._refreshRandomCardsDiv();
        this._refreshBanListDiv();
    };

    _refreshDeckSizeDivs = function() {
        this._mainDeckSizeDiv.innerText = `Main: ${this.mainDeckSize.min} - ${this.mainDeckSize.max}`;
        this._extraDeckSizeDiv.innerText = `Extra: ${this.extraDeckSize.min} - ${this.extraDeckSize.max}`;
    };

    _refreshRandomCardsDiv = function() {
        let randomCardsToAddText = "neither deck";

        if (this.mainDeckAddRandomCardsIfNeeded && this.extraDeckAddRandomCardsIfNeeded) {
            randomCardsToAddText = "both decks";
        } else if (this.mainDeckAddRandomCardsIfNeeded) {
            randomCardsToAddText = "only the main deck";
        } else if (this.extraDeckAddRandomCardsIfNeeded) {
            randomCardsToAddText = "only the extra deck";
        }

        this._randomCardsDiv.innerText = `Adding random cards to: ${randomCardsToAddText}`;
    };

    _refreshBanListDiv = function() {
        if (this.ignoreBanList) {
            this._banListDiv.innerText = "Ignoring the ban list.";
        } else {
            this._banListDiv.innerText = "Following the ban list.";
        }
    };

    // ================== EDITS ==================
    /**
     * Gets the edit div for the base deck
     * This does NOT include the edit div for the filter!
     * @returns The edit div
     */
    getEditDiv = function() {
        let editDiv = dce("div");
        let mainDeckRangeDiv = UIHelpers.createRangeDiv(this, "Main", "mainDeckSize", 40, 60);
        let extraDeckRangeDiv = UIHelpers.createRangeDiv(this, "Extra", "extraDeckSize", 0, 15);
        let mainDeckExtraCardsDiv = UIHelpers.createCheckboxDiv(this, "Main deck add random cards at end", "mainDeckAddRandomCardsIfNeeded");
        let extraDeckExtraCardsDiv = UIHelpers.createCheckboxDiv(this, "Extra deck add random cards at end", "extraDeckAddRandomCardsIfNeeded");
        let banListDiv = UIHelpers.createCheckboxDiv(this, "Ignore ban list", "ignoreBanList");

        editDiv.appendChild(mainDeckRangeDiv);
        editDiv.appendChild(extraDeckRangeDiv);
        editDiv.appendChild(mainDeckExtraCardsDiv);
        editDiv.appendChild(extraDeckExtraCardsDiv);
        editDiv.appendChild(banListDiv);
        return editDiv;
    };
};