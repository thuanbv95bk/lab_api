export class AppGlobals {
  public static setLang(lang: string) {
    localStorage.setItem('language', lang);
  }

  public static getLang(): string {
    if (!localStorage.getItem('language')) {
      localStorage.setItem('language', 'vi');
    }
    return localStorage.getItem('language') || '';
  }
}
