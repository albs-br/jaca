//
// Sortable tables script     (by Grauw)
// ======================
// Using standard DOM methods working in an XHTML environment.
//
// License: Public Domain (but feel free to mention my name ;))
//
// Usage:
// * Include the script in your document head: <script type="text/javascript" src="/js/tablesort.js" defer="defer"></script>
// * Put xmlns:g="http://www.grauw.nl/g" on your document tag (or elsewhere in scope).
// * Put the attribute g:sort="yes" on all <th> tags you want to sort the column of.
// * Put onload="registerSort();" on your <body> tag.
//   
// Notes:
// * The g:sort attribute also accepts 'asc' and 'desc' as values, indicating the initial sort order.
// * The sort-text must be direct children of the table cells.
// * The script doesn't take colspan in account
// * This makes the page not validate anymore. Adding to the !DOCTYPE is possible, but cumbersome and
//   pretty involved. The XHTML spec acknowledges this, but says work to address that is in progress.
//   Personally, I suggest not to bother with it, and accept that the page is formally invalid.
//   DTDs don't work well with namespaces anyway.
// 
//

var ns_g = "http://www.grauw.nl/g";

//
// Registers sort handler
//
function registerSort() {
	// check if browser supports the script
	elem = document.documentElement;
	if (elem.hasAttributeNS && elem.getAttributeNS && elem.setAttributeNS &&
		elem.addEventListener && elem.replaceChild) {
		var x = document.getElementsByTagName('th');
		for (var i=0; i < x.length; i++) {
			if (x[i].hasAttributeNS(ns_g, 'sort')) {
				x[i].addEventListener("click", doSort, false);
				x[i].style.cursor = 'pointer';
				if (!x[i].hasAttribute('title')) {
					x[i].setAttribute('title','Click to sort');
				}
			}
		}
	} else {
		window.status = "This page has sortable tables, however your browser does not support it. Get Mozilla, Firefox or Opera 8 for this functionality.";
	}
}

function hasAttributeNS(obj, ns, attr) {
	if (x[i].hasAttributeNS) {
		return x[i].hasAttributeNS(ns_g, 'sort');
	} else {
		return x[i].hasAttribute('g:sort');
	}
}

//
// Function which is triggered when a sortable table head is clicked
//
function doSort(e) {
	var curelem = e.currentTarget;
	var asc;
	
	if (curelem.getAttributeNS(ns_g, 'sort') != 'desc') {
		asc = true;
		curelem.setAttributeNS(ns_g, 'sort', 'desc');
	} else {
		asc = false;
		curelem.setAttributeNS(ns_g, 'sort', 'asc');
	}

	var sortpos = getElemPosition(curelem);
	
	// Find containing table element
	var table = curelem.parentNode;
	while (table.nodeName != 'table') {
		table = table.parentNode;
	}
	
	var sortlist = getSortList(table, sortpos);
	var oldlist = getSortList(table, sortpos);
	
	if (asc) sortlist.sort(doCompareElem);
	else sortlist.sort(doCompareElemDesc);
	
	for (var i=0; i < sortlist.length; i++) {
		oldlist[i].parentNode.parentNode.replaceChild(
				sortlist[i].parentNode.cloneNode(true),
				oldlist[i].parentNode
			);
	}
}


//
// Retrieves the position of the given element within its parent
// @param1 = element node
//
function getElemPosition(elem) {
	var s = elem.parentNode.childNodes;
	var a = 0;
	for (var i=0; i < s.length; i++) {
		if (s[i] == elem) return a;
		if (s[i].nodeType == Node.ELEMENT_NODE) a++;
	}
}


//
// Gets a list of the td's to be sorted
// @param1 = table node
// @param2 = column nr. to sort
//
function getSortList(table, sortpos) {
	// if <tbody> is present, only sort elements inside <tbody>...
	if (table.getElementsByTagName('tbody').length > 0)
		table = table.getElementsByTagName('tbody')[0];

	var s = table.getElementsByTagName('tr');
	var nodelist = new Array();
	var nodelistpos = 0;
	for (var i=0; i < s.length; i++) {
		var a = 0;
		var p = s[i].childNodes;
		for (var j=0; j<p.length; j++) {
			if (p[j].nodeType == Node.ELEMENT_NODE) {
				if (a == sortpos && p[j].nodeName == 'td') {
					nodelist[nodelistpos++] = p[j];
					break;
				}
				a++;
			}
		}
	}
	return nodelist;
}


//
// Interface which compares the two parameter elements
//
function doCompareElem(a, b) {
	text1 = getTextForCompare(a);
	text2 = getTextForCompare(b);

	if (text1 == text2) return 0;
	else if (text1 > text2) return 1;
	else return -1;
}


//
// Same as above, but reverse order
//
function doCompareElemDesc(a, b) {
	return doCompareElem(b, a);
}


//
// This function retrieves the text inside an element, for comparison.
//
function getTextForCompare(elem) {
	return elem.innerHTML.replace(/<.*?>/g, '');
}



