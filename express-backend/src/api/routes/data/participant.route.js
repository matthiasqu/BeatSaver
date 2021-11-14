const express = require('express');
const validate = require('express-validation');
const controller = require('../../controllers/participant.controller');
const {
  listParticipants,
  createParticipant,
} = require('../../validations/participant.validation');

const router = express.Router();

router.route('/')
/**
   * @api {post} data/participants Create Participant
   * @apiDescription Create a new participant
   * @apiVersion 1.0.0
   * @apiName CreateParticipant
   * @apiGroup Participant
   * @apiPermission admin
   *
   * @apiParam  {Number}                        id          Participant's email
   * @apiParam  {Date}                          date        Participant's password
   * @apiParam  {String=control,experimental}   [gruop]     Participant's group
   * @apiParam  {Blocks}                        blocks      Particpant's played blocks
   * @apiParam  {velocities}                    velocities  Particpant's velocities
   *
   * @apiSuccess (Created 201) {String}  id         Participant's id
   * @apiSuccess (Created 201) {String}  date       Participant's date
   * @apiSuccess (Created 201) {String}  group      Participant's group
   * @apiSuccess (Created 201) {Date}    createdAt  Timestamp
   *
   * @apiError (Bad Request 400)   ValidationError  Some parameters may contain invalid values
   * @apiError (Unauthorized 401)  Unauthorized     Only authenticated users can create the data
   * @apiError (Forbidden 403)     Forbidden        Only admins can create the data
   */
  .post(validate(createParticipant), controller.create)
  .get(validate(listParticipants), controller.list);

module.exports = router;
