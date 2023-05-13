import { Component, Input, OnInit } from '@angular/core';
import { IVoting } from 'src/app/_models/voting';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent {
  @Input() voting: IVoting = {} as IVoting;
}
