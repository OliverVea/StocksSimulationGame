using Core.Models;

namespace Core.Messages;

public record SimulationSteppedMessage(SimulationStep SimulationStep) : Message;