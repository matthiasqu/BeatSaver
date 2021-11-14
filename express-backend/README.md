# Local API for recording participant data
This Express Backend is based on [this repository](https://github.com/danielfsousa/express-rest-boilerplate) which uses NodeJS, ExpressJS, and MongoDB  as well as various middleware. It can easily be containerized using docker, which is the preferred way of spawning MongoDB and Node. The setup is identical, only a new API has been added. The original readme can be found below.

# General workflow

This requires NodeJS to work. If you don't have it installed yet, install Node by downloading it [here](https://nodejs.org/en/download/) or use your favorite package manager such like [Chocolatery](https://chocolatey.org/) (Windows) or [Homebrew](https://brew.sh/index_de) (OSX).

Install Yarn using `npm install --global yarn`, then proceed by following the instructions

Then follow the instructions from the original repository.

## Startup

Unless you have a MongoDB instance running locally, it is recommended to use docker. If necessary, install [Docker Desktop](https://www.docker.com/products/docker-desktop) (again, use a package manager if available).

Then run `yarn docker:dev` while inside `express-backend/`. This should start a MongoDB instance and the express app.

If the startup fails stating that email fields must not be empty, open the `express-backend/.env` file and insert any data here. This does not have to be an actual email server, as it will not be used throughout this project. Simply make sure the values are not empty.

# Source

`express-backend/src/` contains an `index.js` file which contains the entry point for the backend, an `api/` and a `config/` folder. 

## Index.js

Loads the actual express app from `config/express.js`, creates a connection to the database and starts listening.

As soon as the app is ready, the console will print `info: server started on port {port}...`, where the port defaults to 3000. You can then connect via 
`localhost:port/`.

## Config/

### Express.js

Exports the actual express application. Although it supports authentication out of the box, this is not used for recording participant data in this project. Make sure that the data/ api is enabled by checking for this line: 
`app.use('/data', dataRoutes);`

### Vars.js

Exports all variables used throughout the project. These are set in the `express-backend/.env` used by docker. 

### Logger.js

Uses and exports and instance of the [Winston](https://github.com/winstonjs/winston) logging library. 

### Mongoose.js

Uses the [Mongoose](https://mongoosejs.com/) package to connect to the MongoDB instance and exports a connection to it.

### Passport.js

Uses [Passport.js](http://www.passportjs.org/packages/passport-jwt/), to provide OAuth and JSON Web Token (JWT) authentication.

## API/

### Models/

Stores Mongoose Schemas which are used inside MongoDB. 

> The `participant.model.js` file is the only one of interest here. 

It defines a Schema for Participant data, which contains the following fields:

```js
const participantSchema = new mongoose.Schema({
  id: {
    type: Number,
    required: true,
    unique: true,
    trim: true,
    lowercase: true
  },
  date: {
    type: {type: Date, default: Date.now}  
  },
  group: groups,
  blocks: [{color: String, lineIndex: Number, rowIndex: Number, hit: Boolean, velocity: Number, time: Number}],
  velocities: [{hand: String, value: Number}]
}, {
  timestamps: true,
});
```

It furthermore overrides the [transform()](https://mongoosejs.com/docs/api.html#schematypeoptions_SchemaTypeOptions-transform) method and adds some statics to the participant schema.

* group -> refers to the experimental groups possible (either control or experimental in this case)
* get(id) -> tries to retrieve a participant document from the db corresponding to the supplied id.
* list() -> retrieves a list of all participants sorted by their creation timestamp.

### Controllers/

The `participant.controller.js` file defines behavior for actual calls to the API. It imports the `participant.model` and exports methods for retrieving, listing, loading, and creating new participants in the DB.

### Validations/

Uses the [Joi](https://github.com/sideway/joi) package for data validation. This is to ensure that all incoming and outcoming data from the API are of a consistent format.

The `participant.validation.js` file defines a validation for the query of the `listParticipants()` and the body of `createParticipant()` calls defined in `participant.controller.js`. 

### Routes/

Consists of the `v1/` and `data/` directories. The `v1/` directory is the example API from the original boilerplate. The `data/` folder defines the API for tracking data in this project. 

**Note:** Although the boilerplate comes with authentication, it is not used in the `data/`, but in the `v1/` API.

> The `participant.route.js` file defines the routes express is using as endpoints.

It uses the validation methods defined in `participant.validation.js`, and the exported controller from `participant.controller.js`. It defines a single endpoint `data/`, which can be used to create participant documents in the database using POST. Using GET, one can receive all data from all participants.

### Others

Are not important in this scope.


# Inside Unity

Data is sent to the backend after fetching the service from the local network. Additionally, the data is also saved locally. The path defaults to `Quest 2/Internal Memory/Android/data/com.MatthiasQuass.Masterarbeit.BeatSaver/files/Sessions/$DATE$/`.

# Express ES2017 REST API Boilerplate
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com) [![npm version](https://badge.fury.io/js/express-rest-es2017-boilerplate.svg)](https://badge.fury.io/js/express-rest-es2017-boilerplate) [![Build Status](https://travis-ci.org/danielfsousa/express-rest-es2017-boilerplate.svg?branch=master)](https://travis-ci.org/danielfsousa/express-rest-es2017-boilerplate) [![Coverage Status](https://coveralls.io/repos/github/danielfsousa/express-rest-es2017-boilerplate/badge.svg?branch=master)](https://coveralls.io/github/danielfsousa/express-rest-es2017-boilerplate?branch=master)

Boilerplate/Generator/Starter Project for building RESTful APIs and microservices using Node.js, Express and MongoDB

## Features

 - No transpilers, just vanilla javascript
 - ES2017 latest features like Async/Await
 - CORS enabled
 - Uses [yarn](https://yarnpkg.com)
 - Express + MongoDB ([Mongoose](http://mongoosejs.com/))
 - Consistent coding styles with [editorconfig](http://editorconfig.org)
 - [Docker](https://www.docker.com/) support
 - Uses [helmet](https://github.com/helmetjs/helmet) to set some HTTP headers for security
 - Load environment variables from .env files with [dotenv](https://github.com/rolodato/dotenv-safe)
 - Request validation with [joi](https://github.com/hapijs/joi)
 - Gzip compression with [compression](https://github.com/expressjs/compression)
 - Linting with [eslint](http://eslint.org)
 - Tests with [mocha](https://mochajs.org), [chai](http://chaijs.com) and [sinon](http://sinonjs.org)
 - Code coverage with [istanbul](https://istanbul.js.org) and [coveralls](https://coveralls.io)
 - Git hooks with [husky](https://github.com/typicode/husky) 
 - Logging with [morgan](https://github.com/expressjs/morgan)
 - Authentication and Authorization with [passport](http://passportjs.org)
 - API documentation generation with [apidoc](http://apidocjs.com)
 - Continuous integration support with [travisCI](https://travis-ci.org)
 - Monitoring with [pm2](https://github.com/Unitech/pm2)

## Requirements

 - [Node v7.6+](https://nodejs.org/en/download/current/) or [Docker](https://www.docker.com/)
 - [Yarn](https://yarnpkg.com/en/docs/install)

## Getting Started

#### Clone the repo and make it yours:

```bash
git clone --depth 1 https://github.com/danielfsousa/express-rest-es2017-boilerplate
cd express-rest-es2017-boilerplate
rm -rf .git
```

#### Install dependencies:

```bash
yarn
```

#### Set environment variables:

```bash
cp .env.example .env
```

## Running Locally

```bash
yarn dev
```

## Running in Production

```bash
yarn start
```

## Lint

```bash
# lint code with ESLint
yarn lint

# try to fix ESLint errors
yarn lint:fix

# lint and watch for changes
yarn lint:watch
```

## Test

```bash
# run all tests with Mocha
yarn test

# run unit tests
yarn test:unit

# run integration tests
yarn test:integration

# run all tests and watch for changes
yarn test:watch

# open nyc test coverage reports
yarn coverage
```

## Validate

```bash
# run lint and tests
yarn validate
```

## Logs

```bash
# show logs in production
pm2 logs
```

## Documentation

```bash
# generate and open api documentation
yarn docs
```

## Docker

```bash
# run container locally
yarn docker:dev

# run container in production
yarn docker:prod

# run tests
yarn docker:test
```

## Deploy

Set your server ip:

```bash
DEPLOY_SERVER=127.0.0.1
```

Replace my Docker username with yours:

```bash
nano deploy.sh
```

Run deploy script:

```bash
yarn deploy
```

## Tutorials
 - [Create API Documentation Using Squarespace](https://selfaware.blog/home/2018/6/23/api-documentation)

## Inspirations

 - [KunalKapadia/express-mongoose-es6-rest-api](https://github.com/KunalKapadia/express-mongoose-es6-rest-api)
 - [diegohaz/rest](https://github.com/diegohaz/rest)

## License

[MIT License](README.md) - [Daniel Sousa](https://github.com/danielfsousa)
