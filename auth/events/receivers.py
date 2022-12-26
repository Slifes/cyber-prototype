from django_ethereum_events.chainevents import AbstractEventReceiver


class EventTokenTransfer(AbstractEventReceiver):

    def save(self, decoded_event):
        print(decoded_event)
