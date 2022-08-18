export function getBrowserLanguage(): string {
    return navigator.language;
}

export function setHtmlLanguage(language: string): void {
    document.documentElement.lang = language;
}
