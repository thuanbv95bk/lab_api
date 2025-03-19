export class Menu {
  href: string;
  code: string;
  constructor(obj?: Partial<Menu>) {
    this.href = obj?.href || '';
    this.code = obj?.code || '';
  }
}

export class News {
  index: number | null;
  imageUrl: string;
  title: string;
  shortContent: string;
  link: string;
  constructor(obj?: Partial<News>) {
    this.index = obj?.index || null;
    this.imageUrl = obj?.imageUrl || '';
    this.title = obj?.title || '';
    this.shortContent = obj?.shortContent || '';
    this.link = obj?.link || '';
  }
}

export class Branch {
  index: number | null;
  name: string;
  address: SubAddress[];
  constructor(obj?: Partial<Branch>) {
    this.index = obj?.index || null;
    this.name = obj?.name || '';
    this.address = obj?.address || [];
  }
}

export class SubAddress {
  index: number | null;
  Add: string;
  constructor(obj?: Partial<SubAddress>) {
    this.index = obj?.index || null;
    this.Add = obj?.Add || '';
  }
}

export class Languages {
  code: string;
  name: string;
  flag: string;
  constructor(obj?: Partial<Languages>) {
    this.code = obj?.code || '';
    this.name = obj?.name || '';
    this.flag = obj?.flag || '';
  }
}
export class UserInfo {
  userName: string;
  passWord: string;
  isRememberMe: boolean;

  constructor(obj?: Partial<UserInfo>) {
    this.userName = obj?.userName || '';
    this.passWord = obj?.passWord || '';
    this.isRememberMe = obj?.isRememberMe || false;
  }
}
