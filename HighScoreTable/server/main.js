import {Meteor} from 'meteor/meteor';
import {check} from "meteor/check";
import {Scores} from "../imports/scores";

import '../imports/scores.js';

Meteor.publish('scores', function() {
  return Scores.find({}, {sort: {score: -1}});
});

Meteor.methods({
  'scores.insert'(data) {
    check(data, {name: String, score: String});

    let name = data.name;
    let score = parseInt(data.score);

    if (isNaN(score)) {
      throw new Meteor.Error('Provided score string must contain an int.');
    }

    Scores.insert({
      name,
      score
    });
  }
});
