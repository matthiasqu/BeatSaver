const httpStatus = require('http-status');
const Participant = require('../models/participant.model');


/**
 * Load participant and append to req.
 * @public
 */
exports.load = async (req, res, next, id) => {
  try {
    const participant = await Participant.get(id);
    req.locals = { participant };
    return next();
  } catch (error) {
    return next(error);
  }
};

/**
 * Get participant
 * @public
 */
exports.get = (req, res) => res.json(req.locals.participant.transform());

/**
 * Create new participant
 * @public
 */
exports.create = async (req, res, next) => {
  try {
    const participant = new Participant(req.body);
    const savedParticipant = await participant.save();
    res.status(httpStatus.CREATED);
    res.json(savedParticipant.transform());
  } catch (error) {
    next(error);
  }
};

/**
 * Get participant list
 * @public
 */
exports.list = async (req, res, next) => {
  try {
    const participants = await Participant.list(req.query);
    const transformedPrticipants = participants.map(participant => participant.transform());
    res.json(transformedPrticipants);
  } catch (error) {
    next(error);
  }
};
