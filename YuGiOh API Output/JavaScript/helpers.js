Array.prototype.shuffle = function()
{
	var i = this.length;
	while (i)
	{
		var j = Math.floor(Math.random() * i);
		var t = this[--i];
		this[i] = this[j];
		this[j] = t;
	}
	return this;
};

removeFromArray = function(array, value) {
	const index = array.indexOf(value);
	if (index > -1) {
		array.splice(index, 1);
	}
};

uniqifyArray = function(array) {
	return array.filter(function(value, index, self) {
		return self.indexOf(value) === index;
	});
};

/**
 * Taken from:
 * http://mcgivery.com/htmlelement-pseudostyle-settingmodifying-before-and-after-in-javascript/
 */
var UID = {
	_current: 0,
	getNew: function(){
		this._current++;
		return this._current;
	}
};
HTMLElement.prototype.pseudoStyle = function(element,prop,value){
	var _this = this;
	var _sheetId = "pseudoStyles";
	var _head = document.head || document.getElementsByTagName('head')[0];
	var _sheet = document.getElementById(_sheetId) || document.createElement('style');
	_sheet.id = _sheetId;
	var className = "pseudoStyle" + UID.getNew();
	
	_this.className +=  " "+className; 
	
	_sheet.innerHTML += " ."+className+":"+element+"{"+prop+":"+value+"}";
	_head.appendChild(_sheet);
	return this;
};

dce = function dce(tagName, cssClass)
{
    let element = document.createElement(tagName);
    
    if (cssClass) {
    	addCssClass(element, cssClass);
    }
    
    return element;
};

containsCssClass = function containsCssClass(element, classToCheck)
{
    return element && element.className.split(" ").indexOf(classToCheck) > -1;
};

addCssClass = function addCssClass(element, classToAdd)
{
    if (!containsCssClass(element, classToAdd))
    {
        element.className += " " + classToAdd;
    }
};

removeCssClass = function removeCssClass(element, classToRemove)
{
    if (containsCssClass(element, classToRemove))
    {
        var classNames = element.className.split(" ");
        var index = classNames.indexOf(classToRemove);
        classNames.splice(index, 1);
        element.className = classNames.join(" ");
    }
};

replaceCssClass = function replaceCssClass(element, oldClass, newClass)
{
    removeCssClass(element, oldClass);
    addCssClass(element, newClass);
};

toggleCssClass = function toggleCssClass(element, cssClass)
{
	if (containsCssClass(element, cssClass)) {
		removeCssClass(element, cssClass);
		return false;
	} else {
		addCssClass(element, cssClass);
		return true;
	}
};

addOrRemoveCssClass = function addOrRemoveCssClass(element, cssClass, shouldAddCssClass) {
	if (shouldAddCssClass) {
		addCssClass(element, cssClass);
	} else {
		removeCssClass(element, cssClass);
	}
};

//Taken from https://medium.com/@tkssharma/objects-in-javascript-object-assign-deep-copy-64106c9aefab
//TOOD: this doesn't deep copy arrays
cloneObject = function cloneObject(obj) {
  var clone = {};
  for(var i in obj) {
      if (obj[i] != null && typeof(obj[i]) == "object") {
          clone[i] = cloneObject(obj[i]);
      }
      else if (obj[i] != null && typeof(obj[i] == "array")) {
    	  clone[i] = [];
    	  obj[i].forEach(function(element) {
    		  clone[i].push(element);
    	  });
      }
      else {
    	  clone[i] = obj[i];
      }
  }
  return clone;
}