import { AnswerDefinition } from "./answer-definition.model";

export class QuestionDefinition {
  questionText: string;
  hasCorrectAnswer: boolean;
  isDisabled: boolean;
  answers: AnswerDefinition[];
}

