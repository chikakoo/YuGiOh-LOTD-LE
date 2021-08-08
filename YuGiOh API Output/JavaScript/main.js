Main = {
    _uniqueTypes: {}, // Helps with figuring out the C# part of things
    _uniqueBanListValues: {},

    generateString: function() {
        //this._verifyCardsExist();
        this._displayCardData();
    },

    /**
     * Only needs to be used if new cards are added
     */
    _verifyCardsExist: function() {
        let allNames = DataFromAPI.map(x => x.name.toLowerCase());
        Object.keys(Data).forEach(function(cardName) {
            if (!allNames.includes(cardName.toLowerCase())) {
                console.log(`${cardName} not found!`);
            }
        });
    },

    /**
     * Displays all the card data in object form
     */
    _displayCardData: function() {
        let _this = this;
        let objectString = "";
        Object.keys(Data).forEach(function(cardName) {
            let data = DataFromAPI.find(x => x.name.toLowerCase() === cardName.toLowerCase());

            if (!_this._uniqueTypes[data.type]) {
                _this._uniqueTypes[data.type] = data.type;
            }

            if (data.banlist_info && data.banlist_info.ban_tcg && !_this._uniqueBanListValues[data.banlist_info.ban_tcg]) {
                _this._uniqueBanListValues[data.banlist_info.ban_tcg] = data.banlist_info.ban_tcg;
            }

            objectString += "{ ";
            objectString += `"id": ${Data[cardName]}, `;
            objectString += _this._getPropertyString(data, "name");
            objectString += _this._getPropertyString(data, "type");
            objectString +=  _this._getPropertyString(data, "attribute");
            objectString += _this._getPropertyString(data, "level", true);
            objectString += _this._getPropertyString(data, "race");
            objectString +=  _this._getPropertyString(data, "atk", true);
            objectString += _this._getPropertyString(data, "def", true);
            objectString += _this._getPropertyString(data, "archetype");
            objectString += _this._getPropertyString(data, "banlist_info");
            objectString = objectString.substring(0, objectString.length - 2);
            objectString += " },<br/>";
        });

        document.body.innerHTML = objectString;
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