const Joi = require('joi');
const Participant = require('../models/participant.model');

module.exports = {
  listParticipants: {
    query: {
      page: Joi.number().min(1),
      perPage: Joi.number().min(1).max(100),
      id: Joi.number(),
      group: Joi.string().valid(Participant.groups),
      date: Joi.date(),
    },
  },
  createParticipant: {
    body: {
      id: Joi.number().required(),
      group: Joi.string().valid(Participant.groups).required(),
      date: Joi.date().required(),
      detectedBlocks: Joi.array().items(Joi.object({
        color: Joi.string().required(),
        lineIndex: Joi.number().required(),
        rowIndex: Joi.number().required(),
        hit: Joi.boolean().required(),
        velocity: Joi.number(),
        time: Joi.number().required(),
        expectedCutDirection: Joi.string().required(),
        detectedCutDirection: Joi.string().required(),
      })).required(),
      velocities: Joi.array().items(Joi.object({
        hand: Joi.string().required(),
        value: Joi.number().required(),
        time: Joi.number().required(),
      })),
    },
  },
};
