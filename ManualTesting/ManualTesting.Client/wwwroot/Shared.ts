export function setPointerCapture(targetElement: HTMLObjectElement, pointerId: number) {
    targetElement.setPointerCapture(pointerId);
}

export function releasePointerCapture(targetElement: HTMLObjectElement, pointerId: number) {
    targetElement.releasePointerCapture(pointerId);
}


export function getCookies(): string {
    return document.cookie;
}

export function setCookie(key: string, value: string, days: number) {
    const date = new Date();
    date.setTime(date.getTime() + days * 86400000); // 86400000 = 24 * 60 * 60 * 1000
    document.cookie = key + "=" + value + "; expires=" + date.toUTCString() + "; samesite=lax; secure; path=/";
}
