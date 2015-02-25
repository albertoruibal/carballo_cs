using System;
using System.IO;
using System.Text;
using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Book;
using Com.Alonsoruibal.Chess.Log;
using Com.Alonsoruibal.Chess.Search;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Uci
{
	/// <summary>UCI Interface TODO ponder</summary>
	public class Uci : SearchObserver
	{
		internal Config config;

		internal SearchEngineThreaded engine;

		internal bool needsReload = true;

		public Uci()
		{
			Logger.noLog = true;
			// Disable logging
			config = new Config();
			config.SetBook(new ResourceBook("Carballo.book_small.bin"));
		}

		internal virtual void Loop()
		{			try
			{
				while (true)
				{
					string @in = Console.ReadLine();
					string[] tokens = @in.Split(" ");
					int index = 0;
					string command = tokens[index++].ToLower();
					if ("uci".Equals(command))
					{
						System.Console.Out.WriteLine("id name Carballo Chess Engine v1.1");
						System.Console.Out.WriteLine("id author Alberto Alonso Ruibal");
						System.Console.Out.WriteLine("option name Hash type spin default " + Config.DefaultTranspositionTableSize
							 + " min 16 max 256");
						System.Console.Out.WriteLine("option name OwnBook type check default " + Config.DefaultUseBook
							);
						System.Console.Out.WriteLine("option name Null Move type check default " + Config
							.DefaultNullMove);
						System.Console.Out.WriteLine("option name Null Move Margin type spin default " + 
							Config.DefaultNullMoveMargin + " min 0 max 1000");
						System.Console.Out.WriteLine("option name Static Null Move type check default " +
							 Config.DefaultStaticNullMove);
						System.Console.Out.WriteLine("option name LMR type check default " + Config.DefaultLmr
							);
						System.Console.Out.WriteLine("option name IID type check default " + Config.DefaultIid
							);
						System.Console.Out.WriteLine("option name IID Margin type spin default " + Config
							.DefaultIidMargin + " min 1 max 1000");
						System.Console.Out.WriteLine("option name Extensions Check type spin default " + 
							Config.DefaultExtensionsCheck + " min 0 max 2");
						System.Console.Out.WriteLine("option name Extensions Pawn Push type spin default "
							 + Config.DefaultExtensionsPawnPush + " min 0 max 2");
						System.Console.Out.WriteLine("option name Extensions Passed Pawn type spin default "
							 + Config.DefaultExtensionsPassedPawn + " min 0 max 2");
						System.Console.Out.WriteLine("option name Extensions Mate Threat type spin default "
							 + Config.DefaultExtensionsMateThreat + " min 0 max 2");
						System.Console.Out.WriteLine("option name Extensions Recapture type spin default "
							 + Config.DefaultExtensionsRecapture + " min 0 max 2");
						System.Console.Out.WriteLine("option name Extensions Singular type spin default "
							 + Config.DefaultExtensionsSingular + " min 0 max 2");
						System.Console.Out.WriteLine("option name Singular Extension Margin type spin default "
							 + Config.DefaultSingularExtensionMargin + " min 0 max 300");
						System.Console.Out.WriteLine("option name Evaluator type combo default " + Config
							.DefaultEvaluator + " var simplified var complete var experimental");
						System.Console.Out.WriteLine("option name Aspiration Window type check default " 
							+ Config.DefaultAspirationWindow);
						System.Console.Out.WriteLine("option name Aspiration Window Sizes type string default "
							 + Config.DefaultAspirationWindowSizes);
						System.Console.Out.WriteLine("option name Futility type check default " + Config.
							DefaultFutility);
						System.Console.Out.WriteLine("option name Futility Margin type spin default " + Config
							.DefaultFutilityMargin + " min 1 max 1000");
						System.Console.Out.WriteLine("option name Aggressive Futility type check default "
							 + Config.DefaultAggresiveFutility);
						System.Console.Out.WriteLine("option name Aggressive Futility Margin type spin default "
							 + Config.DefaultAggresiveFutilityMargin + " min 1 max 1000");
						System.Console.Out.WriteLine("option name Futility Margin QS spin default " + Config
							.DefaultFutilityMarginQs + " min 1 max 1000");
						System.Console.Out.WriteLine("option name Razoring type check default " + Config.
							DefaultRazoring);
						System.Console.Out.WriteLine("option name Razoring Margin type spin default " + Config
							.DefaultRazoringMargin + " min 1 max 1000");
						System.Console.Out.WriteLine("option name Contempt Factor type spin default " + Config
							.DefaultContemptFactor + " min -200 max 200");
						System.Console.Out.WriteLine("option name Eval Center type spin default " + Config
							.DefaultEvalCenter + " min 0 max 200");
						System.Console.Out.WriteLine("option name Eval Positional type spin default " + Config
							.DefaultEvalPositional + " min 0 max 200");
						System.Console.Out.WriteLine("option name Eval Attacks type spin default " + Config
							.DefaultEvalAttacks + " min 0 max 200");
						System.Console.Out.WriteLine("option name Eval Mobility type spin default " + Config
							.DefaultEvalMobility + " min 0 max 200");
						System.Console.Out.WriteLine("option name Eval Pawn Structure type spin default "
							 + Config.DefaultEvalPawnStructure + " min 0 max 200");
						System.Console.Out.WriteLine("option name Eval Passed Pawns type spin default " +
							 Config.DefaultEvalPassedPawns + " min 0 max 200");
						System.Console.Out.WriteLine("option name Eval King Safety type spin default " + 
							Config.DefaultEvalKingSafety + " min 0 max 200");
						System.Console.Out.WriteLine("option name Rand type spin default " + Config.DefaultRand
							 + " min 0 max 100");
						System.Console.Out.WriteLine("uciok");
					}
					else
					{
						if ("setoption".Equals(command))
						{
							index++; // Skip name
							// get the option name without spaces
							StringBuilder nameSB = new StringBuilder();
							string tok;
							while (!"value".Equals(tok = tokens[index++]))
							{
								nameSB.Append(tok);
							}
							string name = nameSB.ToString();
							string value = tokens[index++];
							if ("Hash".Equals(name))
							{
								config.SetTranspositionTableSize(System.Convert.ToInt32(value));
							}
							else
							{
								if ("OwnBook".Equals(name))
								{
									config.SetUseBook(System.Boolean.Parse(value));
								}
								else
								{
									if ("NullMove".Equals(name))
									{
										config.SetNullMove(System.Boolean.Parse(value));
									}
									else
									{
										if ("NullMoveMargin".Equals(name))
										{
											config.SetNullMoveMargin(System.Convert.ToInt32(value));
										}
										else
										{
											if ("StaticNullMove".Equals(name))
											{
												config.SetStaticNullMove(System.Boolean.Parse(value));
											}
											else
											{
												if ("IID".Equals(name))
												{
													config.GetIid(System.Boolean.Parse(value));
												}
												else
												{
													if ("IIDMargin".Equals(name))
													{
														config.SetIidMargin(System.Convert.ToInt32(value));
													}
													else
													{
														if ("ExtensionsCheck".Equals(name))
														{
															config.SetExtensionsCheck(System.Convert.ToInt32(value));
														}
														else
														{
															if ("ExtensionsPawnPush".Equals(name))
															{
																config.SetExtensionsPawnPush(System.Convert.ToInt32(value));
															}
															else
															{
																if ("ExtensionsPassedPawn".Equals(name))
																{
																	config.SetExtensionsPassedPawn(System.Convert.ToInt32(value));
																}
																else
																{
																	if ("ExtensionsMateThreat".Equals(name))
																	{
																		config.SetExtensionsMateThreat(System.Convert.ToInt32(value));
																	}
																	else
																	{
																		if ("ExtensionsRecapture".Equals(name))
																		{
																			config.SetExtensionsRecapture(System.Convert.ToInt32(value));
																		}
																		else
																		{
																			if ("ExtensionsSingular".Equals(name))
																			{
																				config.SetExtensionsSingular(System.Convert.ToInt32(value));
																			}
																			else
																			{
																				if ("SingularExtensionMargin".Equals(name))
																				{
																					config.SetSingularExtensionMargin(System.Convert.ToInt32(value));
																				}
																				else
																				{
																					if ("Evaluator".Equals(name))
																					{
																						config.SetEvaluator(value);
																					}
																					else
																					{
																						if ("AspirationWindow".Equals(name))
																						{
																							config.SetAspirationWindow(System.Boolean.Parse(value));
																						}
																						else
																						{
																							if ("AspirationWindowSizes".Equals(name))
																							{
																								config.SetAspirationWindowSizes(value);
																							}
																							else
																							{
																								if ("Futility".Equals(name))
																								{
																									config.SetFutility(System.Boolean.Parse(value));
																								}
																								else
																								{
																									if ("FutilityMargin".Equals(name))
																									{
																										config.SetFutilityMargin(System.Convert.ToInt32(value));
																									}
																									else
																									{
																										if ("AggressiveFutility".Equals(name))
																										{
																											config.SetAggressiveFutility(System.Boolean.Parse(value));
																										}
																										else
																										{
																											if ("AggressiveFutilityMargin".Equals(name))
																											{
																												config.SetAggressiveFutilityMargin(System.Convert.ToInt32(value));
																											}
																											else
																											{
																												if ("FutilityMarginQS".Equals(name))
																												{
																													config.SetFutilityMarginQS(System.Convert.ToInt32(value));
																												}
																												else
																												{
																													if ("Razoring".Equals(name))
																													{
																														config.SetRazoring(System.Boolean.Parse(value));
																													}
																													else
																													{
																														if ("RazoringMargin".Equals(name))
																														{
																															config.SetRazoringMargin(System.Convert.ToInt32(value));
																														}
																														else
																														{
																															if ("ContemptFactor".Equals(name))
																															{
																																config.SetContemptFactor(System.Convert.ToInt32(value));
																															}
																															else
																															{
																																if ("EvalCenter".Equals(name))
																																{
																																	config.SetEvalCenter(System.Convert.ToInt32(value));
																																}
																																else
																																{
																																	if ("EvalPositional".Equals(name))
																																	{
																																		config.SetEvalPositional(System.Convert.ToInt32(value));
																																	}
																																	else
																																	{
																																		if ("EvalAttacks".Equals(name))
																																		{
																																			config.SetEvalAttacks(System.Convert.ToInt32(value));
																																		}
																																		else
																																		{
																																			if ("EvalMobility".Equals(name))
																																			{
																																				config.SetEvalMobility(System.Convert.ToInt32(value));
																																			}
																																			else
																																			{
																																				if ("EvalPawnStructure".Equals(name))
																																				{
																																					config.SetEvalPawnStructure(System.Convert.ToInt32(value));
																																				}
																																				else
																																				{
																																					if ("EvalPassedPawns".Equals(name))
																																					{
																																						config.SetEvalPassedPawns(System.Convert.ToInt32(value));
																																					}
																																					else
																																					{
																																						if ("EvalKingSafety".Equals(name))
																																						{
																																							config.SetEvalKingSafety(System.Convert.ToInt32(value));
																																						}
																																						else
																																						{
																																							if ("Rand".Equals(name))
																																							{
																																								config.SetRand(System.Convert.ToInt32(value));
																																							}
																																						}
																																					}
																																				}
																																			}
																																		}
																																	}
																																}
																															}
																														}
																													}
																												}
																											}
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
							needsReload = true;
						}
						else
						{
							if ("isready".Equals(command))
							{
								if (needsReload)
								{
									engine = new SearchEngineThreaded(config);
									engine.SetObserver(this);
									needsReload = false;
									System.GC.Collect();
								}
								else
								{
									// Wait for the engine to finish searching
									while (engine.IsSearching())
									{
										try
										{
											Sharpen.Thread.Sleep(100);
										}
										catch (Exception)
										{
										}
									}
								}
								System.Console.Out.WriteLine("readyok");
							}
							else
							{
								if ("quit".Equals(command))
								{
									System.Environment.Exit(0);
								}
								else
								{
									if ("go".Equals(command))
									{
										SearchParameters searchParameters = new SearchParameters();
										while (index < tokens.Length)
										{
											string arg1 = tokens[index++];
											if ("searchmoves".Equals(arg1))
											{
											}
											else
											{
												// TODO
												if ("ponder".Equals(arg1))
												{
													searchParameters.SetPonder(true);
												}
												else
												{
													if ("wtime".Equals(arg1))
													{
														searchParameters.SetWtime(System.Convert.ToInt32(tokens[index++]));
													}
													else
													{
														if ("btime".Equals(arg1))
														{
															searchParameters.SetBtime(System.Convert.ToInt32(tokens[index++]));
														}
														else
														{
															if ("winc".Equals(arg1))
															{
																searchParameters.SetWinc(System.Convert.ToInt32(tokens[index++]));
															}
															else
															{
																if ("binc".Equals(arg1))
																{
																	searchParameters.SetBinc(System.Convert.ToInt32(tokens[index++]));
																}
																else
																{
																	if ("movestogo".Equals(arg1))
																	{
																		searchParameters.SetMovesToGo(System.Convert.ToInt32(tokens[index++]));
																	}
																	else
																	{
																		if ("depth".Equals(arg1))
																		{
																			searchParameters.SetDepth(System.Convert.ToInt32(tokens[index++]));
																		}
																		else
																		{
																			if ("nodes".Equals(arg1))
																			{
																				searchParameters.SetNodes(System.Convert.ToInt32(tokens[index++]));
																			}
																			else
																			{
																				if ("mate".Equals(arg1))
																				{
																					searchParameters.SetMate(System.Convert.ToInt32(tokens[index++]));
																				}
																				else
																				{
																					if ("movetime".Equals(arg1))
																					{
																						searchParameters.SetMoveTime(System.Convert.ToInt32(tokens[index++]));
																					}
																					else
																					{
																						if ("infinite".Equals(arg1))
																						{
																							searchParameters.SetInfinite(true);
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
										engine.Go(searchParameters);
									}
									else
									{
										if ("stop".Equals(command))
										{
											engine.Stop();
										}
										else
										{
											if ("ucinewgame".Equals(command))
											{
												engine.GetBoard().StartPosition();
												engine.Clear();
											}
											else
											{
												if ("position".Equals(command))
												{
													if (index < tokens.Length)
													{
														string arg1 = tokens[index++];
														if ("startpos".Equals(arg1))
														{
															engine.GetBoard().StartPosition();
														}
														else
														{
															if ("fen".Equals(arg1))
															{
																// FEN string may have spaces
																StringBuilder fenSb = new StringBuilder();
																while (index < tokens.Length)
																{
																	if ("moves".Equals(tokens[index]))
																	{
																		break;
																	}
																	fenSb.Append(tokens[index++]);
																	if (index < tokens.Length)
																	{
																		fenSb.Append(" ");
																	}
																}
																engine.GetBoard().SetFen(fenSb.ToString());
															}
														}
													}
													if (index < tokens.Length)
													{
														string arg1 = tokens[index++];
														if ("moves".Equals(arg1))
														{
															while (index < tokens.Length)
															{
																int move = Move.GetFromString(engine.GetBoard(), tokens[index++], true);
																engine.GetBoard().DoMove(move);
															}
														}
													}
												}
												else
												{
													if ("debug".Equals(command))
													{
													}
													else
													{
														if ("ponderhit".Equals(command))
														{
														}
														else
														{
															// TODO ponder not supported
															if ("register".Equals(command))
															{
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (IOException e)
			{
				// not used
				// System.out.println("Command not recognized: " + in);
				Sharpen.Runtime.PrintStackTrace(e);
			}
		}

		public virtual void BestMove(int bestMove, int ponder)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("bestmove ");
			sb.Append(Move.ToString(bestMove));
			if (ponder != 0 && ponder != -1)
			{
				sb.Append(" ponder ");
				sb.Append(Move.ToString(ponder));
			}
			System.Console.Out.WriteLine(sb.ToString());
		}

		public virtual void Info(SearchStatusInfo info)
		{
			System.Console.Out.Write("info ");
			System.Console.Out.WriteLine(info.ToString());
		}

		public static void Main(string[] args)
		{
			Com.Alonsoruibal.Chess.Uci.Uci uci = new Com.Alonsoruibal.Chess.Uci.Uci();
			uci.Loop();
		}
	}
}
