let UIHelpers = {
    _id: 0,

    createRangeDiv: function(context, label, property, min, max) {
        let minVal = min || 0;
        let maxVal = max || undefined;

        let rangeContainer = dce("div", "ui-range");

        let rangeLabel = dce("label");
        rangeLabel.for = ++this._id;
        rangeLabel.innerText = `${label}:`;

        let rangeSeparator = dce("div");
        rangeSeparator.innerText = "-";

        let rangeTextLower = this._createNumberInput(`${this._id}-lower`, minVal, maxVal);
        let rangeTextUpper = this._createNumberInput(`${this._id}-upper`, minVal, maxVal);

        rangeTextLower.onblur = function(event) {
            if (rangeTextLower.value) {
                rangeTextUpper.value = Math.max(rangeTextUpper.value, rangeTextLower.value);
            } else {
                rangeTextLower.value = rangeTextUpper.value;
            }

            context[property].min = Number(rangeTextLower.value);
            context[property].max = Number(rangeTextUpper.value);
        };

        rangeTextUpper.onblur = function(event) {
            if (rangeTextUpper.value) {
                rangeTextLower.value = Math.min(rangeTextUpper.value, rangeTextLower.value);
            } else {
                rangeTextUpper.value = rangeTextLower.value;
            }

            context[property].min = Number(rangeTextLower.value);
            context[property].max = Number(rangeTextUpper.value);
        };

        let initialValue = context[property];
        rangeTextLower.value = initialValue.min;
        rangeTextUpper.value = initialValue.max;

        rangeContainer.appendChild(rangeLabel);
        rangeContainer.appendChild(rangeTextLower);
        rangeContainer.appendChild(rangeSeparator);
        rangeContainer.appendChild(rangeTextUpper);
        return rangeContainer;
    },

    _createNumberInput: function(id, min, max) {
        let input = dce("input");
        input.for = id;
        input.name = id;
        input.type = "number"
        input.min = min;
        if (max) {
            input.max = max;
        }

        return input;
    },

    createCheckboxDiv: function(context, label, property) {
        let checkboxDiv = dce("div", "ui-checkbox");

        let checkbox = dce("input");
        checkbox.type = "checkbox";
        checkbox.name = ++this._id;

        let initialValue = context[property];
        checkbox.checked = !!initialValue;

        checkbox.onchange = function(event) {
            context[property] = checkbox.checked;
        };

        let checkboxLabel = dce("label");
        checkboxLabel.appendChild(checkbox);
        checkboxDiv.appendChild(checkboxLabel);

        checkboxLabel.insertAdjacentText("beforeend", label)
        return checkboxDiv;
    },

    createCheckboxSetDiv: function(context, label, property, options) {
        let checkboxSetDiv = dce("div", "ui-checkbox-set");

        let labelDiv = dce("div");
        labelDiv.innerText = label;

        checkboxSetDiv.appendChild(labelDiv);

        let _this = this;
        options.forEach(function(option) {
            let checkbox = dce("input");
            checkbox.type = "checkbox";
            checkbox.name = ++_this._id;

            checkbox.onchange = function(event) {
                let selections = context[property];
                const index = selections.indexOf(option);
                if (index > -1) {
                    selections.splice(index, 1);
                }

                if (checkbox.checked) {
                    selections.push(option);
                }
            }

            if (context[property].includes(option)) {
                checkbox.checked = true;
            }

            let checkboxLabel = dce("label");
            checkboxLabel.appendChild(checkbox);
            checkboxSetDiv.appendChild(checkboxLabel);

            checkboxLabel.insertAdjacentText("beforeend", option);
        });

        return checkboxSetDiv;
    },

    /**
     * Creates a radio button div
     * @param {Any} context - the context
     * @param {String} label - the label to display above the radio buttons
     * @param {String} property - the property to bind the selected to (a string, in context.property)
     * @param {Array<String>} options - an array of options to display
     * @returns The created HTMLElement
     */
    createRadioButtonDiv: function(context, label, property, options) {
        if (options.length === 0) {
            console.log(`ERROR: Tried to create radio buttons with no options - label: ${label}`);
            return null;
        }

        let radioContainer = dce("div", "ui-radio");

        let labelDiv = dce("div");
        labelDiv.innerText = label;

        let radioId = ++this._id;

        radioContainer.appendChild(labelDiv);

        let initialValue = context[property];
        options.forEach(function(option) {
            let radioInput = dce("input");
            radioInput.type = "radio";
            radioInput.name = radioId;
            radioInput.id = option;

            let optionDiv = dce("label");
            optionDiv.setAttribute("for", option);
            optionDiv.innerText = option;
            optionDiv.name = radioId;

            if (option === initialValue) {
                radioInput.checked = true;
            }

            radioInput.onchange = function(event) {
                context[property] = option;
            }

            radioContainer.appendChild(radioInput);
            radioContainer.appendChild(optionDiv);
        });

        return radioContainer;
    },

    createTextFieldDiv: function(context, label, property) {
        let textFieldContainer = dce("div", "ui-text-field");

        let textFieldLabel = dce("label");
        textFieldLabel.innerText = `${label}:`;

        let textField = dce("input");
        textField.onblur = function(event) {
            context[property] = textField.value;
        }

        let initialValue = context[property];
        if (initialValue) {
            textField.value = initialValue;
        }

        textFieldContainer.appendChild(textFieldLabel);
        textFieldContainer.appendChild(textField);
        return textFieldContainer;
    }
};