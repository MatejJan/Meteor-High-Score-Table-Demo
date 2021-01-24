import { Template } from 'meteor/templating';
import { ReactiveVar } from 'meteor/reactive-var';

import './main.html';
import {Scores} from "../imports/scores";

Template.body.onCreated(function() {
});

Template.body.helpers({
  scores() {
    return Scores.find({}, {sort: {score: -1}});
  },
});

Template.body.events({
  'submit .new-score-form'(event, instance) {
    event.preventDefault();

    var form = event.target;
    Scores.insert(
      {
        name: form.name.value,
        score: parseInt(form.score.value)
      }
    );
  },
});
