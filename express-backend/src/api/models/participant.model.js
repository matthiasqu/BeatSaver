const mongoose = require('mongoose');
const httpStatus = require('http-status');
const { omitBy, isNil } = require('lodash');
const APIError = require('../utils/APIError');

/**
* User Roles
*/
const group = ['control', 'experimental'];
const color = ['Red', 'Blue'];
const lineIndex = [0, 1, 2, 3];
const rowIndex = [0, 1, 2];
const hand = ['left', 'right'];

/**
 * Participant Schema
 * @private
 */
const participantSchema = new mongoose.Schema({
  id: {
    type: Number,
    required: true,
    unique: true,
    trim: true,
    lowercase: true,
  },
  date: {
    type: { type: Date, default: Date.now },
  },
  group,
  detectedBlocks: [{
    color,
    expectedCutDirection: String,
    detectedCutDirection: String,
    lineIndex,
    rowIndex,
    hit: Boolean,
    velocity: Number,
    time: Number,
  }],
  velocities: [{ hand, value: Number, time: Number }],
}, {
  timestamps: true,
});

/**
 * Add your
 * - pre-save hooks
 * - validations
 * - virtuals
 */

/**
 * Methods
 */
participantSchema.method({
  transform() {
    const transformed = {};
    const fields = ['id', 'date', 'group', 'detectedBlocks', 'velocities', 'createdAt'];
    fields.forEach((field) => {
      transformed[field] = this[field];
    });

    return transformed;
  },
});

/**
 * Statics
 */
participantSchema.statics = {

  groups: group,
  color,
  lineIndex,
  rowIndex,
  hand,
  /**
   * Get participant
   *
   * @param {ObjectId} id - The objectId of participant.
   * @returns {Promise<participant, APIError>}
   */
  async get(id) {
    try {
      let participant;

      if (mongoose.Types.ObjectId.isValid(id)) {
        participant = await this.findById(id).exec();
      }
      if (participant) {
        return participant;
      }

      throw new APIError({
        message: 'participant does not exist',
        status: httpStatus.NOT_FOUND,
      });
    } catch (error) {
      throw error;
    }
  },

  /**
   * List participants in descending order of 'createdAt' timestamp.
   *
   * @param {number} skip - Number of participants to be skipped.
   * @param {number} limit - Limit number of participants to be returned.
   * @returns {Promise<participant[]>}
   */
  list({
    page = 1, perPage = 30, id,
  }) {
    const options = omitBy({ id }, isNil);

    return this.find(options)
      .sort({ createdAt: -1 })
      .skip(perPage * (page - 1))
      .limit(perPage)
      .exec();
  },
};

/**
 * @typedef Participant
 */
module.exports = mongoose.model('Participant', participantSchema);
