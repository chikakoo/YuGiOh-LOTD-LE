class Filter {
    displayDiv = null;

    isMain = false;
    isInDeckCreationTask = false;
    deckCreationTaskRef = null;

    _id = 0;

    generalCardTypes = [];
    excludedGeneralTypes = [];
    nameFilterType = Enums.NameFitlerTypes.WILDCARD;
    nameFilter = "";
    cardTypes = [];
    monsterTypeFilter = Enums.MonsterTypeFilter.NONE;
    levelRange = { min: -1, max: -1 };
    allowLevel0IfNotInRange = false;
    attackRange = { min: -1, max: -1 };
    defenseRange = { min: -1, max: -1 };
    archetypes = ""; //TODO: will be an array eventually? right now it's comma-delimited and will be formatted into an array
    types = [];
    attributes = [];

    _generalCardTypesDiv = null;
    _excludedGeneralTypesDiv = null;
    _nameFilterDiv = null;
    _cardTypesDiv = null;
    _monsterTypeFilterDiv = null;
    _monsterLevelRangeDiv = null;
    _monsterStatRangeDiv = null;
    _archetypesDiv = null;
    _typesDiv = null;
    _attributesDiv = null;

    constructor(isMain, isInDeckCreationTask, deckCreationTaskRef, id) { 
        this.isMain = isMain;
        this.isInDeckCreationTask = isInDeckCreationTask;
        this.deckCreationTaskRef = deckCreationTaskRef;
        this._id = id;
    };

    getDisplayDiv = function() {
        this.displayDiv = dce("div", "deck-display");

        if (this.isMain) {
            this.displayDiv.innerText = "Main Filter";
        }

        let _this = this;
        this.displayDiv.onclick = function(event) {
            event.stopPropagation();
            Main.editComponent(_this);
        };

        this.displayDiv.oncontextmenu = function(event) {
            event.stopPropagation();
            event.preventDefault();

            if (_this.isInDeckCreationTask && !_this.isMain) {
                let menuOptions = [];
                menuOptions.push({
                    text: "Move filter up",
                    callback: function() {
                        _this.deckCreationTaskRef.moveFilterUp(_this);
                    }
                });

                menuOptions.push({
                    text: "Move filter down",
                    callback: function() {
                        _this.deckCreationTaskRef.moveFilterDown(_this);
                    }
                });

                menuOptions.push({
                    text: "Delete filter",
                    callback: function() {
                        _this.deckCreationTaskRef.deleteFilter(_this);
                    }
                });

                ContextMenu.show(menuOptions, event);
            }
        };

        this._initializeDiv();
        this.refreshMainDisplayDiv();

        return this.displayDiv;
    };

    _initializeDiv = function() {
        this._generalCardTypesDiv = dce("div");
        this._excludedGeneralTypesDiv = dce("div");
        this._nameFilterDiv = dce("div");
        this._cardTypesDiv = dce("div");
        this._monsterTypeFilterDiv = dce("div");
        this._monsterLevelRangeDiv = dce("div");
        this._monsterStatRangeDiv = dce("div");
        this._archetypesDiv = dce("div");
        this._typesDiv = dce("div");
        this._attributesDiv = dce("div");

        this.displayDiv.appendChild(this._generalCardTypesDiv);
        this.displayDiv.appendChild(this._excludedGeneralTypesDiv);
        this.displayDiv.appendChild(this._nameFilterDiv);
        this.displayDiv.appendChild(this._cardTypesDiv);
        this.displayDiv.appendChild(this._monsterTypeFilterDiv);
        this.displayDiv.appendChild(this._monsterLevelRangeDiv);
        this.displayDiv.appendChild(this._monsterStatRangeDiv);
        this.displayDiv.appendChild(this._archetypesDiv);
        this.displayDiv.appendChild(this._typesDiv);
        this.displayDiv.appendChild(this._attributesDiv);
    };

    refreshMainDisplayDiv = function() {
        addOrRemoveCssClass(this._generalCardTypesDiv, "nodisp", this.generalCardTypes.length === 0);
        addOrRemoveCssClass(this._excludedGeneralTypesDiv, "nodisp", this.excludedGeneralTypes.length === 0);
        addOrRemoveCssClass(this._nameFilterDiv, "nodisp", this.nameFilter.trim() === "");
        addOrRemoveCssClass(this._cardTypesDiv, "nodisp", this.cardTypes.length === 0);
        addOrRemoveCssClass(this._monsterTypeFilterDiv, "nodisp", this.monsterTypeFilter === Enums.MonsterTypeFilter.NONE);
        addOrRemoveCssClass(this._monsterLevelRangeDiv, "nodisp", this.levelRange.min < 0 || this.levelRange.max < 0);
        addOrRemoveCssClass(this._monsterStatRangeDiv, "nodisp", Math.min(this.attackRange.min, this.attackRange.max) < 0 && Math.min(this.defenseRange.min, this.defenseRange.max) < 0);
        addOrRemoveCssClass(this._archetypesDiv, "nodisp", this.archetypes.trim() === ""); //TODO: will eventually be an array
        addOrRemoveCssClass(this._typesDiv, "nodisp", this.types.length === 0);
        addOrRemoveCssClass(this._attributesDiv, "nodisp", this.attributes.length === 0);

        this._generalCardTypesDiv.innerText = `General card types: ${this.generalCardTypes.join(", ")}`;
        this._excludedGeneralTypesDiv.innerText = `Excluded general types: ${this.excludedGeneralTypes.join(", ")}`;
        this._nameFilterDiv.innerText = `Name filter: ${this.nameFilterType} (${this.nameFilter})`;
        this._cardTypesDiv.innerText = `Card types: ${this.cardTypes.join(", ")}`;
        this._monsterTypeFilterDiv.innerText = this.monsterTypeFilter;
        this._monsterLevelRangeDiv.innerText = `${this._getRangeString("Monster level", this.levelRange)}${this.allowLevel0IfNotInRange ? " (Includes level 0)" : ""}`;
        this._monsterStatRangeDiv.innerText = `${this._getRangeString("ATK", this.attackRange)}    ${this._getRangeString("DEF", this.defenseRange)}`.trim();
        this._archetypesDiv.innerText = `Archetypes: ${this.archetypes}`;
        this._typesDiv.innerText = `Card types (races): ${this.types.join(", ")}`;
        this._attributesDiv.innerText = `Attributes: ${this.attributes.join(", ")}`;
    };

    _getRangeString = function(label, range) {
        if (range.min < 0 && range.max < 0) {
            return "";
        }

        return `${label}: ${range.min} - ${range.max}`;
    };

    getEditDiv = function() {
        let editDiv = dce("div");

        let generalCardTypesDiv = UIHelpers.createCheckboxSetDiv(this, "General card types", "generalCardTypes", Object.values(Enums.GeneralCardTypes));
        let excludedGeneralTypes = UIHelpers.createCheckboxSetDiv(this, "Excluded General Types", "excludedGeneralTypes", Object.values(Enums.GeneralCardTypes));
        let nameFilterTypes = UIHelpers.createRadioButtonDiv(this, "Name filter type", "nameFilterType", Object.values(Enums.NameFitlerTypes));
        let nameFilterDiv = UIHelpers.createTextFieldDiv(this, "Name filter", "nameFilter");
        let cardTypesDiv = UIHelpers.createCheckboxSetDiv(this, "Card types", "cardTypes", Object.values(Enums.CardTypes));
        let monsterTypeFilterDiv = UIHelpers.createRadioButtonDiv(this, "Monster filter", "monsterTypeFilter", Object.values(Enums.MonsterTypeFilter));
        let levelRangeDiv = UIHelpers.createRangeDiv(this, "Level range", "levelRange", -1, 12);
        let allowLevel0Div = UIHelpers.createCheckboxDiv(this, "Allow level 0 if not in range", "allowLevel0IfNotInRange");
        let attackRangeDiv = UIHelpers.createRangeDiv(this, "Attack range", "attackRange", -1, 10000);
        let defenseRangeDiv = UIHelpers.createRangeDiv(this, "Defense range", "defenseRange", -1, 10000);
        let archetypesDiv = UIHelpers.createTextFieldDiv(this, "Archetypes", "archetypes"); //TODO: get a list of all of these... for now, comma delimited?
        let typesDiv = UIHelpers.createCheckboxSetDiv(this, "Monster/race types", "types", Object.values(Enums.RaceTypes));
        let attributesDiv = UIHelpers.createCheckboxSetDiv(this, "Attributes", "attributes", Object.values(Enums.Attributes)) //TODO: get the real list of these... store it on the C# code too

        editDiv.appendChild(generalCardTypesDiv);
        editDiv.appendChild(excludedGeneralTypes);
        editDiv.appendChild(nameFilterTypes);
        editDiv.appendChild(nameFilterDiv);
        editDiv.appendChild(cardTypesDiv);
        editDiv.appendChild(monsterTypeFilterDiv);
        editDiv.appendChild(levelRangeDiv);
        editDiv.appendChild(allowLevel0Div);
        editDiv.appendChild(attackRangeDiv);
        editDiv.appendChild(defenseRangeDiv);
        editDiv.appendChild(archetypesDiv);
        editDiv.appendChild(typesDiv);
        editDiv.appendChild(attributesDiv);
        
        return editDiv;
    };
}