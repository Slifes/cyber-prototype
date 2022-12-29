from django.conf import settings
from django.core.management.base import BaseCommand

from django_ethereum_events.models import MonitoredEvent


event = "Transfer"  # the emitted event name
event_receiver = "events.receivers.EventTokenTransfer"
contract_address = settings.HEART_TOKEN  # the address of the contract emitting the event


def load_abi():
    with open("./contracts/hrt.json", "r") as f:
        return f.read()


class Command(BaseCommand):
    help = 'Add monitored event'

    def handle(self, *args, **options):
        MonitoredEvent.objects.register_event(
            event_name=event,
            contract_address=contract_address,
            contract_abi=load_abi(),
            event_receiver=event_receiver
        )
