export class QuestionDefinition {
  questionText: string;
  hasCorrectAnswer: boolean;
  isDisabled: boolean;
  answers: [
    {
      answerText: string;
      imagePath: string;
      isCorrectAnswer: boolean;
      isDisabled: boolean;
    }
  ];
}

