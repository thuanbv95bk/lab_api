export class AppGlobals {
  public static readonly LANGUAGE = 'language';
  // Set Ngôn ngữ
  public static setLang(lang: string) {
    localStorage.setItem(this.LANGUAGE, lang);
  }

  // lấy ra ngôn ngữ đang lưu
  public static getLang(): string {
    if (!localStorage.getItem(this.LANGUAGE)) {
      localStorage.setItem(this.LANGUAGE, 'vi');
    }
    return localStorage.getItem(this.LANGUAGE) || '';
  }
}
