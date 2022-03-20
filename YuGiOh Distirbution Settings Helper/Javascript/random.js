let Random = {
    /**
     * Gets a random number between the min and max, inclusive
     */
    getRandomNumber: function(min, max) {
        min = Number(min);
        max = Number(max);
        if (max < min) {
            max = min;
        }
        return Math.floor(Math.random() * (max - min + 1)) + min;
    },

    /**
     * Gets a random boolean
     */
    getRandomBoolean: function() {
        return this.getRandomNumber(0, 1) === 1;
    },

    /**
     * Gets a random boolean based on a passed in percentage for it to be true
     */
    getRandomBooleanFromPercentage: function(percentage) {
        let number = this.getRandomNumber(0, 99);
        return number < percentage;
    },

    /**
     * Gets a random key from the given object
     */
    getRandomKeyFromObject: function(object) {
        let keys = Object.keys(object);
        if (keys.length > 0) {
            let randomIndex = this.getRandomNumber(0, keys.length - 1);
            return keys[randomIndex];
        }
        return null;
    },
    
    /**
     * Gets a random entry from the given object
     */
    getRandomEntryFromObject: function(object) {
        let key = this.getRandomKeyFromObject(object);
        if (!key) {
            return null;
        }

        return object[key];
    },

    /**
     * Gets a random value from the given array
     */
    getRandomValueFromArray: function(array) {
        let index = this.getRandomNumber(0, array.length - 1);
        return array[index];
    },

    /**
     * Gets and removes a random value from the given array
     */
    getAndRemoveRandomValueFromArray: function(array) {
        let index = this.getRandomNumber(0, array.length - 1);
        return array.splice(index, 1)[0];
    },

    /**
     * Gets a set of random values from an array
     * Returns the minimum amount if the number passed if more than the array has to offer
     * @param array - the array
     * @param randomValues - the random values
     */
    getRandomValuesFromArray: function(array, numberOfValues) {
        numberOfValues = Math.min(array.length, numberOfValues);
        if (numberOfValues < 1) {
            return [];
        }

        let arrayCopy = [];
        array.forEach(function(value) {
            arrayCopy.push(value);
        });

        let output = [];
        for (let i = 0; i < numberOfValues; i++) {
            output.push(this.getAndRemoveRandomValueFromArray(arrayCopy));
        }

        return output;
    },

    /**
     * Gets a hex string of a random color
     * @returns The hex color string
     */
    getRandomColorHexString: function() {
        return `#${Random.getRandomNumber(0,255).toString(16).padStart(2, '0')}${Random.getRandomNumber(0,255).toString(16).padStart(2, '0')}${Random.getRandomNumber(0,255).toString(16).padStart(2, '0')}`;
    },

    /**
     * Gets a list of hex strings of a random color
     * @param amount - the amount of strings to get
     * @returns The hex color strings
     */
    getRandomColorHexStrings: function(amount) {
        let strings = [];
        for (let i = 0; i < amount; i++) {
            strings.push(this.getRandomColorHexString());
        }
        return strings;
    },

    /**
     * Generates a string with ASCII characters with values from 32 - 126
     * Table: http://www.asciitable.com/
     * @param length - the length of the string
     */
    getRandomASCIIString: function(length) {
        let output = "";
        for (let i = 0; i < length; i++) {
            output += String.fromCharCode(this.getRandomNumber(32, 126));
        }
        return output;
    }
};