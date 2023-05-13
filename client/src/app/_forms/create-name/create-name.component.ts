import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IVoting } from 'src/app/_models/voting';
import { VotingService } from 'src/app/_services/voting.service';

@Component({
  selector: 'app-create-name',
  templateUrl: './create-name.component.html',
  styleUrls: ['./create-name.component.css']
})
export class CreateNameComponent {
  @Input() voting: IVoting = {} as IVoting;
  @Output() cancelCreate = new EventEmitter();
  createForm: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
  }


  cancel() {
    this.cancelCreate = new EventEmitter(false);
  }

  create() {
    // this.votingService.addVariant(this.voting.id, this.createForm?.value).subscribe({
    //   next: () => {
    //     this.router.navigateByUrl('/votings/my/edit/' + this.voting.id);
    //   }
    // });
  }

  initializeForm() {
    this.createForm = this.formBuilder.group({
      title: new FormControl('', Validators.required),
      details: new FormControl('', [Validators.required]),
      description: new FormControl('', [Validators.required])
    });
  }
}
