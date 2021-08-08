/**
 * Leave prompt
 */
 window.onbeforeunload = function() {
    return "Please make sure you save before leaving!";
  };
  

Main = {
    _currentIndex: 0,
    _currentCardInfo: {},
    _uniqueTypes: {}, // Helps with figuring out the C# part of things
    _uniqueBanListValues: {},
    _uniqueRaces: {},

    generateString: function() {
        if (Settings.verifyCardsExist) {
            this._verifyCardsExist();
        }
        
        if (Settings.cardSortingMode) {
            this._startSortMode();
        } else {
            this._displayCardData();
        }
    },

    /**
     * Only needs to be used if new cards are added
     */
    _verifyCardsExist: function() {
        let allNames = DataFromAPI.map(x => x.name);
        Object.keys(Data).forEach(function(cardName) {
            if (!allNames.includes(cardName)) {
                console.log(`${cardName} not found!`);
            }
        });
    },

    /**
     * Starts the sort mode, where we will manually set cards as banned via tempban
     */
    _startSortMode: function() {
        removeCssClass(document.getElementById("cardInformation"), "nodisp");
        this._clearCardInfoDiv();

        this._currentIndex = Settings.indexToStartAt;
        this._displayCardInfoForNextCard();
    },

    /**
     * Writes the current card to the div to copy later
     * Does not mark the card as banned
     */
    onCardIsGoodClicked: function() {
        let objectString = this._createObjectString(this._currentCardInfo.name, this._currentCardInfo);
        document.getElementById("mainDiv").innerHTML += objectString;
        this._displayCardInfoForNextCard();
    },

    /**
     * Writes the current card to the div to copy later
     * Marks the card as banned
     */
    onCardIsBadClicked: function() {
        let objectString = this._createObjectString(this._currentCardInfo.name, this._currentCardInfo, true);
        document.getElementById("mainDiv").innerHTML += objectString;
        this._displayCardInfoForNextCard();
    },

    /**
     * Displays the card information for the next card
     */
    _displayCardInfoForNextCard() {
        this._currentCardInfo = this._getNextCard();
        this._displayCardInfo(this._currentCardInfo)
    },

    /**
     * Displays all the info from the card in the appropriate divs
     */
    _displayCardInfo(cardInfo) {
        document.getElementById("indexValue").innerHTML = this._currentIndex;
        document.getElementById("idValue").innerHTML = Data[cardInfo.name];
        document.getElementById("nameValue").innerHTML = cardInfo.name;

        document.getElementById("levelValue").innerHTML = cardInfo.level;
        document.getElementById("raceValue").innerHTML = cardInfo.race;
        document.getElementById("typeValue").innerHTML = cardInfo.type;
        document.getElementById("attributeValue").innerHTML = cardInfo.attribute;

        document.getElementById("atkValue").innerHTML = cardInfo.atk;
        document.getElementById("defValue").innerHTML = cardInfo.def;
        document.getElementById("archetypeValue").innerHTML = cardInfo.archetype;
        document.getElementById("banlist_infoValue").innerHTML = cardInfo.banlist_info ? cardInfo.banlist_info.ban_tcg : "undefined";

        document.getElementById("descriptionValue").innerHTML = cardInfo.desc;
    },

    /** Creates a clean slate for the card info div */
    _clearCardInfoDiv: function() {
        document.getElementById("indexValue").innerHTML = "<no value>";
        document.getElementById("idValue").innerHTML = "<no value>";
        document.getElementById("nameValue").innerHTML = "<no value>";

        document.getElementById("levelValue").innerHTML = "<no value>";
        document.getElementById("raceValue").innerHTML = "<no value>";
        document.getElementById("typeValue").innerHTML = "<no value>";
        document.getElementById("attributeValue").innerHTML = "<no value>";

        document.getElementById("atkValue").innerHTML = "<no value>";
        document.getElementById("defValue").innerHTML = "<no value>";
        document.getElementById("archetypeValue").innerHTML = "<no value>";
        document.getElementById("banlist_infoValue").innerHTML = "<no value>";

        document.getElementById("descriptionValue").innerHTML = "<no value>";
    },

    /**
     * Gets the next card to display - will skip over those not in the game
     * @returns The card data to display
     */
    _getNextCard: function() {
        let cardData = null;
        do {
            if (this._currentIndex > DataFromAPI.length - 1) {
                break;
            }

            let currentCard = DataFromAPI[this._currentIndex];
            if (Data[currentCard.name]) { // Only if the card is in the game should we use it
                cardData = currentCard;
            }
            this._currentIndex++;
        } while(cardData == null);

        return cardData;
    },

    /**
     * Displays all the card data in object form
     */
    _displayCardData: function() {
        let _this = this;
        let objectString = "";
        Object.keys(Data).forEach(function(cardName) {
            let data = DataFromAPI.find(x => x.name === cardName);

            if (!_this._uniqueTypes[data.type]) {
                _this._uniqueTypes[data.type] = data.type;
            }

            if (data.banlist_info && data.banlist_info.ban_tcg && !_this._uniqueBanListValues[data.banlist_info.ban_tcg]) {
                _this._uniqueBanListValues[data.banlist_info.ban_tcg] = data.banlist_info.ban_tcg;
            }

            if (!_this._uniqueRaces[data.race]) {
                _this._uniqueRaces[data.race] = data.race;
            }

            objectString += _this._createObjectString(cardName, data);
        });

        document.getElementById("mainDiv").innerHTML = objectString;
    },

    /**
     * Creates the string for the card's JSON object
     * @param {String} cardName - the name of the card
     * @param {*} data - the card's data, from DataFromAPI
     * @param {Boolean} banCard - whether to ban the card altogether
     */
    _createObjectString: function(cardName, data, banCard) {
        let objectString = "{ ";
        objectString += `"id": ${Data[cardName]}, `;
        objectString += this._getPropertyString(data, "name");
        objectString += this._getPropertyString(data, "type");
        objectString +=  this._getPropertyString(data, "attribute");
        objectString += this._getPropertyString(data, "level", true);
        objectString += this._getPropertyString(data, "race");
        objectString +=  this._getPropertyString(data, "atk", true);
        objectString += this._getPropertyString(data, "def", true);
        objectString += this._getPropertyString(data, "archetype");
        objectString += this._getPropertyString(data, "banlist_info");

        if (banCard) {
            objectString += '"tempBan": true, ';
        }
        
        objectString = objectString.substring(0, objectString.length - 2);
        objectString += " },<br/>";

        return objectString;
    },

    /**
     * Gets the property and value string, if it exists
     * @param {*} data - the data
     * @param {String} property - the property
     * @param {Boolean} isNumberProperty - if true, doesn't add quotes to the value
     */
    _getPropertyString(data, property, isNumberProperty) {
        let value = data[property];
        if (value) {
            if (property === "banlist_info") {
                value = value.ban_tcg;
            }
            if (property === "name") {
                value = value.replaceAll('"', '\\"');
            }
            value = isNumberProperty ? value : `"${value}"`;
            return `"${property}": ${value}, `;
        }
        return "";
    }
};