import { Meteor } from 'meteor/meteor';
import '../imports/scores.js';
import {Scores} from "../imports/scores";

Meteor.startup(() => {
  // code to run on server at startup
});

Meteor.publish('scores', function() {
  return Scores.find({}, {sort: {score: -1}});
});

Meteor.methods({
  'scores.insert'(name, score) {
    check(name, String);
    check(score, Number);

    Scores.insert({
      name,
      score
    });
  }
});
