/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { InMemoryDbService } from 'angular-in-memory-web-api';

export class InMemoryDataService implements InMemoryDbService {
  createDb() {
    const poll = [
      {
        type: 'poll',
        status: 'draft',
        currentQuestion: 1,
        _id: '5ba6b7b99d08b620884a004e',
        name: 'Test Poll',
        pollDefinition: {
          type: 'poll',
          isPublished: false,
          _id: '5ba56f736299071380e61767',
          name: 'Best Microsoft Presenter',
          description: '',
          theme: 'default',
          expiryDate: null,
          questions: [
            {
              hasCorrectAnswer: false,
              isDisabled: false,
              answers: [
                {
                  isCorrectAnswer: true,
                  isDisabled: false,
                  _id: '5ba56f736299071380e6176f',
                  answerText: 'Dan Baker',
                  imagePath: null
                },
                {
                  isCorrectAnswer: false,
                  isDisabled: false,
                  _id: '5ba56f736299071380e6176e',
                  answerText: 'Scott Hanselmanr',
                  imagePath: null
                },
                {
                  isCorrectAnswer: false,
                  isDisabled: false,
                  _id: '5ba56f736299071380e6176d',
                  answerText: 'Scott Guthrie',
                  imagePath: null
                }
              ],
              _id: '5ba56f736299071380e6176c',
              questionText: 'Who is the best overall presenter?'
            },
            {
              hasCorrectAnswer: false,
              isDisabled: false,
              answers: [
                {
                  isCorrectAnswer: true,
                  isDisabled: false,
                  _id: '5ba56f736299071380e6176b',
                  answerText: 'Dan Baker',
                  imagePath: null
                },
                {
                  isCorrectAnswer: false,
                  isDisabled: false,
                  _id: '5ba56f736299071380e6176a',
                  answerText: 'Scott Hanselman',
                  imagePath: null
                },
                {
                  isCorrectAnswer: false,
                  isDisabled: false,
                  _id: '5ba56f736299071380e61769',
                  answerText: 'Scott Guthrie',
                  imagePath: null
                }
              ],
              _id: '5ba56f736299071380e61768',
              questionText: 'Who is the funniest presenter?'
            }
          ],
          createDate: '2018-09-21T22:23:47.073Z',
          __v: 0
        },
        pollDate: '2018-12-11T00:00:00.000Z',
        owner: '5ba4fe19e9e8b91cd4897f6e',
        questions: [
          {
            answers: [
              {
                _id: '5ba6b7b99d08b620884a0056',
                answerId: '5ba56f736299071380e6176f',
                voteCount: 0
              },
              {
                _id: '5ba6b7b99d08b620884a0055',
                answerId: '5ba56f736299071380e6176e',
                voteCount: 0
              },
              {
                _id: '5ba6b7b99d08b620884a0054',
                answerId: '5ba56f736299071380e6176d',
                voteCount: 0
              }
            ],
            _id: '5ba6b7b99d08b620884a0053',
            questionId: '5ba56f736299071380e6176c'
          },
          {
            answers: [
              {
                _id: '5ba6b7b99d08b620884a0052',
                answerId: '5ba56f736299071380e6176b',
                voteCount: 0
              },
              {
                _id: '5ba6b7b99d08b620884a0051',
                answerId: '5ba56f736299071380e6176a',
                voteCount: 0
              },
              {
                _id: '5ba6b7b99d08b620884a0050',
                answerId: '5ba56f736299071380e61769',
                voteCount: 0
              }
            ],
            _id: '5ba6b7b99d08b620884a004f',
            questionId: '5ba56f736299071380e61768'
          }
        ],
        __v: 0
      }
    ];
    return { poll };
  }
}
