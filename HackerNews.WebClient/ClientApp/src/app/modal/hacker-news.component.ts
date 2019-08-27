import {Component, Inject, Injectable} from '@angular/core';

import {MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';
import { HackerNews } from '../models/hacker-news.model';

@Component({
templateUrl:  'hacker-news.component.html'
})
export class HackerNewsComponent {
    constructor(private  dialogRef:  MatDialogRef<HackerNewsComponent>, @Inject(MAT_DIALOG_DATA) public data:  any) {
    }
    public closeMe() {
        this.dialogRef.close();
    }

  convertNumberToDate(value: number) {
    let returnValue = "";
        if (value && typeof value === 'number') {
          const dateValue = new Date(value * 1000);

          returnValue = dateValue.toDateString();
          } else {
          returnValue = 'Item has no date';
          }
    return returnValue;
    }
    decodeEntities(str: string) {
        if (str && typeof str === 'string') {
            // strip script tags
            str = str.replace("<script", "");
            str = str.replace(/&/g, '&amp;');
            str = str.replace(/</g, '&lt;');
            str = str.replace(/>/g, '&gt;');

          } else {
          str = 'Item has no text';
          }
          return str;
      }
}
