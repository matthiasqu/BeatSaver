const express = require('express');
const participantRoutes = require('./participant.route');

const router = express.Router();

/**
 * GET data/status
 */
router.get('/status', (req, res) => res.send('Status OK'));

router.use('/participant', participantRoutes);

module.exports = router;
