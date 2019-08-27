export interface HackerNews {
  id: number;
  type: string;
  deleted: boolean;
  by: string;
  time: number;
  text: string;
  dead: boolean;
  parent: number;
  poll: string;
  kids: number[];
  url: string;
  score: number;
  title: string;
  parts: number[];
  descendants: number;
  date: Date;
}

export interface ItemResults {
  totalCount: number;
  items: HackerNews[];
}