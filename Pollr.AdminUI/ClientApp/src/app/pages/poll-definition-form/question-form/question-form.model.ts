import { FormControl, Validators } from '@angular/forms'
import { QuestionDefinition } from '../../../shared/models/question-definition.model';

export class QuestionForm {
  questionText = new FormControl()
  hasCorrectAnswer = new FormControl()
  isDisabled = new FormControl()

  constructor(
    questionDefinition: QuestionDefinition
  ) {
    this.questionText.setValue(questionDefinition.questionText)
    this.questionText.setValidators([Validators.required])

    this.hasCorrectAnswer.setValue(questionDefinition.hasCorrectAnswer)

    this.isDisabled.setValue(questionDefinition.isDisabled)
  }
}
