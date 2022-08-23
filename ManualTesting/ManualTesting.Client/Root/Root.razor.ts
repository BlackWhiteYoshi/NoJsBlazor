export function getBrowserLanguage(): string {
    return navigator.language;
}

export function setHtmlLanguage(language: string) {
    document.documentElement.lang = language;
}
