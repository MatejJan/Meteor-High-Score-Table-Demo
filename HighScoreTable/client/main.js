import {Template} from 'meteor/templating';
import {Scores} from "../imports/scores";

import './main.html';

Template.body.helpers({
  scores() {
    return Scores.find({}, {sort: {score: -1}});
  },
});

Template.body.events({
  'submit form'(event) {
    event.preventDefault();

    let form = event.target;

    Scores.insert({
      name: form.name.value,
      score: parseInt(form.score.value)
    });
  },
});
