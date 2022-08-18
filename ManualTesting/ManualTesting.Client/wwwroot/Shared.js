export function setPointerCapture(targetElement, pointerId) {
    targetElement.setPointerCapture(pointerId);
}
export function releasePointerCapture(targetElement, pointerId) {
    targetElement.releasePointerCapture(pointerId);
}
export function getCookies() {
    return document.cookie;
}
export function setCookie(key, value, days) {
    const date = new Date();
    date.setTime(date.getTime() + days * 86400000); // 86400000 = 24 * 60 * 60 * 1000
    document.cookie = key + "=" + value + "; expires=" + date.toUTCString() + "; samesite=lax; secure; path=/";
}
