// @ts-check


/**
 * @param {HTMLObjectElement} targetElement
 * @param {number} pointerId
 */
function setPointerCapture(targetElement, pointerId) {
    targetElement.setPointerCapture(pointerId);
}

/**
 * @param {HTMLObjectElement} targetElement
 * @param {number} pointerId
 */
function releasePointerCapture(targetElement, pointerId) {
    targetElement.releasePointerCapture(pointerId);
}
