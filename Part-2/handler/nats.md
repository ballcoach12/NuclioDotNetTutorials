# Nats Notes

* Verified that the body of the event for Nats data is the data field of the Nats message itself.
	* See lines 30-32 in <https://github.com/nuclio/nuclio/blob/bba043b398b7ffc8bb00842c0a123c9524024c41/pkg/processor/trigger/nats/event.go>

* I think we should be able to simply decode the incoming message exactly the way we do in other apps,  perform the calculation, and then re-encode it and send it out.

